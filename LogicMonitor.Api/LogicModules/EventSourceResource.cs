namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The EventSource Resource
/// </summary>
[DataContract]
public class EventSourceResource
{
	/// <summary>
	/// The deviceEventSourceId
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int ResourceEventSourceId { get; set; }

	/// <summary>
	/// The EventSource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;
}
