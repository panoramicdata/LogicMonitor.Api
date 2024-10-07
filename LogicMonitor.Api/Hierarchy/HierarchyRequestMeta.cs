namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyRequestMeta
/// </summary>
[DataContract]
public class HierarchyRequestMeta
{
	/// <summary>
	/// Sort
	/// </summary>
	[DataMember(Name = "sort")]
	public string Sort { get; set; } = string.Empty;

	/// <summary>
	/// ResourceId
	/// </summary>
	[DataMember(Name = "resourceId")]
	public HierarchyTypeAndId ResourceId { get; set; } = new();

	/// <summary>
	/// DataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public HierarchyTypeAndId DataSourceId { get; set; } = new();
}