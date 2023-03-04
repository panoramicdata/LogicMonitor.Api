namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Request object for the parameters needed to retrieve alert history
/// </summary>
public class AlertHistoryRequest : IValidate
{
	/// <summary>
	/// The Id of the alert for which history is being requested
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The start date and time of the history required, in UTC
	/// </summary>
	public DateTime? StartDateTimeUtc { get; set; }

	/// <summary>
	/// The end date and time of the history required, in UTC
	/// </summary>
	public DateTime? EndDateTimeUtc { get; set; }

	///
	public AlertHistoryPeriod HistoryPeriod { get; set; } = AlertHistoryPeriod.Last24Hours;

	/// <summary>
	/// The suburl of the request to be performed
	/// </summary>
	internal string SubUrl
	{
		get
		{
			var start = GetHistoryPeriodStartSeconds();
			var end = GetHistoryPeriodEndSeconds();
			return $"alert/alerts/{Id}/history?start={start}&end={end}";
		}
	}

	/// <summary>
	/// Validation of the request parameters
	/// </summary>
	public void Validate()
	{
		if (Id is null)
		{
			throw new ArgumentNullException(nameof(Id));
		}

		if (HistoryPeriod == AlertHistoryPeriod.Custom)
		{
			if (StartDateTimeUtc is null || EndDateTimeUtc is null)
			{
				throw new ArgumentException("For Custom Alert History, StartDateTime and EndDateTime must both be specified.");
			}

			if (StartDateTimeUtc >= EndDateTimeUtc)
			{
				throw new ArgumentException("Start time must be before end time.");
			}
		}
	}

	/// <summary>
	/// Get the start date and time for the history request
	/// </summary>
	/// <returns>The unix timestamp for the start of the history period</returns>
	private long GetHistoryPeriodStartSeconds()
	{
		if (HistoryPeriod == AlertHistoryPeriod.Custom && StartDateTimeUtc is null)
		{
			throw new ArgumentException("StartDateTime must be specified for a custom history range");
		}

		var start = HistoryPeriod switch
		{
			AlertHistoryPeriod.Custom => StartDateTimeUtc!.Value,
			AlertHistoryPeriod.Last24Hours => DateTime.UtcNow.AddDays(-1),
			AlertHistoryPeriod.Last7Days => DateTime.UtcNow.AddDays(-7),
			AlertHistoryPeriod.Last30Days => DateTime.UtcNow.AddDays(-30),
			_ => throw new ArgumentException(nameof(HistoryPeriod))
		};
		return start.SecondsSinceTheEpoch();
	}

	/// <summary>
	/// Get the end date and time for the history request
	/// </summary>
	/// <returns>The unix timestamp for the end of the history period</returns>
	private long GetHistoryPeriodEndSeconds()
	{
		if (HistoryPeriod == AlertHistoryPeriod.Custom && EndDateTimeUtc is null)
		{
			throw new ArgumentException("EndDateTime must be specified for a custom history range");
		}

		var end = HistoryPeriod switch
		{
			AlertHistoryPeriod.Custom => EndDateTimeUtc!.Value,
			_ => DateTime.UtcNow
		};
		return end.SecondsSinceTheEpoch();
	}
}
