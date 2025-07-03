namespace LogicMonitor.Api.Data;

/// <summary>
/// A request for raw data from a Website Checkpoint
/// </summary>
public class WebsiteCheckPointRawDataRequest : GraphDataRequestBase
{
	internal string SubUrl
	{
		get
		{
			return
				$"website/" +
				$"websites/" +
				$"{WebsiteId}/" +
				$"checkpoints/" +
				$"{WebsiteCheckPointId}/" +
				$"data" +
				$"?{PeriodPart}" +
				$"&aggregate={Aggregation.ToString().ToLowerInvariant()}" +
				(DataPointNames.Count > 0
					? ("&datapoints=" + string.Join(",", DataPointNames))
					: string.Empty);
		}
	}

	/// <summary>
	/// The set of time parameters
	/// </summary>
	protected string PeriodPart
	{
		get
		{
			if (TimePeriod is not null)
			{
				return $"period={(int)TimePeriod}";
			}

			if (StartDateTime is not null && EndDateTime is not null)
			{
				return $"start={StartDateTime?.SecondsSinceTheEpoch()}&end={EndDateTime?.SecondsSinceTheEpoch()}";
			}

			return $"period={(int)WebsiteCheckpointRawDataTimePeriod.OneHour}";
		}
	}

	/// <summary>
	/// The aggregation type to use for the data points
	/// </summary>
	public WebsiteCheckpointRawDataAggregationType Aggregation { get; set; }

	/// <summary>
	/// The DataPoint names. Leave empty to return data for all available data points
	/// </summary>
	public List<string> DataPointNames { get; set; } = [];

	/// <summary>
	/// Optional start date time. When used overrides TimePeriod
	/// </summary>
	public DateTime? StartDateTime { get; set; }

	/// <summary>
	/// Optional end date time. When used overrides TimePeriod
	/// </summary>
	public DateTime? EndDateTime { get; set; }

	/// <summary>
	/// The time period
	/// </summary>
	public WebsiteCheckpointRawDataTimePeriod? TimePeriod { get; set; }

	/// <summary>
	/// The Website Checkpoint ID
	/// </summary>
	public int WebsiteCheckPointId { get; set; }

	/// <summary>
	/// The Website ID
	/// </summary>
	public int WebsiteId { get; set; }

	/// <summary>
	/// Validates the parameters
	/// </summary>
	/// <exception cref="ArgumentException"></exception>
	public override void Validate()
	{
		if (WebsiteId <= 0)
		{
			throw new ArgumentException("WebsiteId must be specified.");
		}

		if (WebsiteCheckPointId <= 0)
		{
			throw new ArgumentException("WebsiteCheckPointId must be specified.");
		}

		if (TimePeriod is null)
		{
			if ((StartDateTime is null || EndDateTime is null))
			{
				throw new ArgumentException("If TimePeriod is not specified, StartDateTime and EndDateTime must both be specified.");
			}

			if (StartDateTime >= EndDateTime)
			{
				throw new ArgumentException("Start time must be before end time.");
			}
		}
	}
}
