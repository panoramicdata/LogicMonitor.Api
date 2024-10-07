namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// ResourceDataSourceInstanceSummary
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceSummary
{
	/// <summary>
	/// Model
	/// </summary>
	[DataMember(Name = "model")]
	public string Model { get; set; } = string.Empty;

	/// <summary>
	/// Id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	///	DisplayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///	Data ID
	/// </summary>
	[DataMember(Name = "dataId")]
	public HierarchyTypeAndId DataId { get; set; } = new HierarchyTypeAndId();
}

