namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// Hierarchy Type and Id
/// </summary>
[DataContract]
public class HierarchyTypeAndId
{
	/// <summary>
	/// Id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Model
	/// </summary>
	[DataMember(Name = "model")]
	public string Type { get; set; } = string.Empty;
}

