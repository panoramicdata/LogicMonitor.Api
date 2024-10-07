namespace LogicMonitor.Api.Hierarchy;

/// <summary>
/// HierarchyResponseMeta
/// </summary>
[DataContract]
public class HierarchyResponseMeta
{
	/// <summary>
	/// Sort
	/// </summary>
	[DataMember(Name = "sort")]
	public string Sort { get; set; } = string.Empty;

	/// <summary>
	/// Total count
	/// </summary>
	[DataMember(Name = "totalCount")]
	public int TotalCount { get; set; }

	/// <summary>
	/// Filtered count
	/// </summary>
	[DataMember(Name = "filteredCount")]
	public int FilteredCount { get; set; }

	/// <summary>
	/// Page size
	/// </summary>
	[DataMember(Name = "perPageCount")]
	public int PageSize { get; set; }

	/// <summary>
	/// Page Offset
	/// </summary>
	[DataMember(Name = "pageOffsetCount")]
	public int PageOffset { get; set; }
}

