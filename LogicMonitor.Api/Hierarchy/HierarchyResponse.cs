namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyResponse
/// </summary>
[DataContract]
public class HierarchyResponse
{
	/// <summary>
	/// The Data
	/// </summary>
	[DataMember(Name = "data")]
	public HierarchyResponseData Data { get; set; } = new();

	/// <summary>
	/// The Meta
	/// </summary>
	[DataMember(Name = "meta")]
	public HierarchyResponseMeta Meta { get; set; } = new();
}
