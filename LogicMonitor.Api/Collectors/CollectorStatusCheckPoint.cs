namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A single check in a collector status check
/// </summary>
[DataContract]
public class CollectorStatusCheckPoint
{
	/// <summary>
	/// The check name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The check detail
	/// </summary>
	[DataMember(Name = "detail")]
	public string Detail { get; set; } = string.Empty;

	/// <summary>
	/// The check status, for example OK
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
