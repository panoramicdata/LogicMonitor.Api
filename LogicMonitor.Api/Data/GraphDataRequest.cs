namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for GraphData
/// </summary>
public abstract class GraphDataRequest : GraphDataRequestBase
{
	/// <summary>
	///    Constructor.
	///    Sets Width to 500 by default
	/// </summary>
	protected GraphDataRequest()
	{
		Width = 500;
	}

	/// <summary>
	///    The graph width
	/// </summary>
	public int Width { get; set; }

	/// <summary>
	///    The graph StartDateTime
	/// </summary>
	public DateTime? StartDateTime { get; set; }

	/// <summary>
	///    The graph EndDateTime
	/// </summary>
	public DateTime? EndDateTime { get; set; }

	/// <summary>
	///    The TimePeriod
	///    Either this should be specified, or both StartDateTime and EndDateTime.
	/// </summary>
	public TimePeriod TimePeriod { get; set; }

	/// <summary>
	///    The query subUrl
	/// </summary>
	internal abstract string SubUrl { get; }

	/// <summary>
	///    The SubUrl TimePart (for use by inheritors)
	/// </summary>
	protected string TimePart => $"time={TimePeriod.ToString().ToLowerInvariant()}&start={StartDateTime?.SecondsSinceTheEpoch()}&end={EndDateTime?.SecondsSinceTheEpoch()}";

	/// <inheritdoc />
	public abstract override void Validate();

	/// <summary>
	/// Internal validation
	/// </summary>
	protected void ValidateInternal()
	{
		switch (TimePeriod)
		{
			case TimePeriod.Unknown:
				throw new ArgumentException("TimePeriod cannot be Unknown if specified.");
			case TimePeriod.Zoom:
				if (StartDateTime is null || EndDateTime is null)
				{
					throw new ArgumentException("If TimePeriod is not specified, StartDateTime and EndDateTime must both be specified.");
				}

				if (StartDateTime >= EndDateTime)
				{
					throw new ArgumentException("Start time must be before end time.");
				}

				break;
			default:
				break;
		}
	}
}
