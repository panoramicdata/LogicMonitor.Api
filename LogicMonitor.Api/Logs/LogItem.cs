namespace LogicMonitor.Api.Logs;

/// <summary>
///    User logged event
/// </summary>
[DataContract]
public class LogItem : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	///    The user that performed the action
	/// </summary>
	[DataMember(Name = "username")]
	public string PerformedByUsername { get; set; } = string.Empty;

	/// <summary>
	///    The DateTime the event happened in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "happenedOn")]
	public long HappenedOnTimeStampUtc { get; set; }

	/// <summary>
	///    The session ID
	/// </summary>
	[DataMember(Name = "sessionId")]
	public string SessionId { get; set; } = string.Empty;

	/// <summary>
	///    Event description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    Event time (local)
	/// </summary>
	[DataMember(Name = "happenedOnLocal")]
	public string HappenedOnLocalString { get; set; } = string.Empty;

	/// <summary>
	///    IP Address
	/// </summary>
	[DataMember(Name = "ip")]
	public string IpAddress { get; set; } = string.Empty;

	/// <summary>
	///    The DateTime the event happened UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime HappenedOnUtc => HappenedOnTimeStampUtc.ToDateTimeUtc();

	/// <summary>
	///    Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{HappenedOnUtc:yyyy-MM-dd}: {PerformedByUsername} - {Description}";

	/// <inheritdoc />
	public string Endpoint() => "setting/accesslogs";
}
