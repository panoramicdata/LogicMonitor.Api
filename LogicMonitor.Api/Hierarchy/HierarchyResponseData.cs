namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyResponseData
/// </summary>
[DataContract]
public class HierarchyResponseData
{
	/// <summary>
	/// All Ids
	/// </summary>
	[DataMember(Name = "allIds")]
	public IReadOnlyCollection<HierarchyTypeAndId> AllIds { get; set; } = [];

	/// <summary>
	/// Items
	/// </summary>
	[DataMember(Name = "items")]
	public object[] Items { get; set; } = [];

	/// <summary>
	/// By ID
	/// </summary>
	[DataMember(Name = "byId")]
	public HierarchyResponseById byId { get; set; } = new HierarchyResponseById();
}