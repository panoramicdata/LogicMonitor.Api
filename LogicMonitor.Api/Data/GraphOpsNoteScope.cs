namespace LogicMonitor.Api.Data;

/// <summary>
/// GraphOpsNoteScope
/// </summary>

[DataContract]
public class GraphOpsNoteScope
{
	/// <summary>
	/// The service group Id
	/// </summary>
	[DataMember(Name = "serviceGroupIds")]
	public List<int> ServiceGroupIds { get; set; } = [];

	/// <summary>
	/// The service id
	/// </summary>
	[DataMember(Name = "serviceId")]
	public int ServiceId { get; set; }

	/// <summary>
	/// device | service | website\nThe corresponding type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The device group Id
	/// </summary>
	[DataMember(Name = "deviceGroupIds")]
	public List<int> DeviceGroupIds { get; set; } = [];

	/// <summary>
	/// The device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
