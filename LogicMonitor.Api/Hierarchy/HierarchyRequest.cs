namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyRequest
/// </summary>
[DataContract]
public class HierarchyRequest
{
	/// <summary>
	/// The Meta
	/// </summary>
	[DataMember(Name = "meta")]
	public HierarchyRequestMeta Meta { get; set; } = new();
}
