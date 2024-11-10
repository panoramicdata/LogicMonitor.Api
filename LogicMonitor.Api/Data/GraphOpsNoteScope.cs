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
	/// The ResourceGroup ids
	/// </summary>
	[DataMember(Name = "deviceGroupIds")]
	public int ResourceGroupIds { get; set; }

	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
