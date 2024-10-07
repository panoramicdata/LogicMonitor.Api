namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyResponseById
/// </summary>
[DataContract]
public class HierarchyResponseById
{
	/// <summary>
	/// DataSourceInstances
	/// </summary>
	[DataMember(Name = "dataSourceInstances")]
	public Dictionary<string, ResourceDataSourceInstanceSummary> ResourceDataSourceInstances { get; set; } = [];
}

