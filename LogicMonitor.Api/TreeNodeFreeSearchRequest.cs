namespace LogicMonitor.Api;

/// <summary>
/// A tree node free search request
/// </summary>
[DataContract]
public class TreeNodeFreeSearchRequest
{
	/// <summary>
	/// The freesearch request type
	/// </summary>
	[DataMember(Name = "type")]
	public TreeNodeFreeSearchRequestType Type { get; set; }

	/// <summary>
	/// The search text
	/// </summary>
	[DataMember(Name = "searchText")]
	public string SearchText { get; set; } = string.Empty;

	/// <summary>
	/// The result limitation
	/// </summary>
	[DataMember(Name = "resultLimitation")]
	public int ResultLimitation { get; set; }
}
