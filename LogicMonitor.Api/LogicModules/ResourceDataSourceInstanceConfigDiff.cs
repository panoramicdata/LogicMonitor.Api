namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// ResourceDataSourceInstanceConfigDiff
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceConfigDiff
{
	/// <summary>
	/// Diff type, values can be : add|remove
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// configuration content
	/// </summary>
	[DataMember(Name = "content")]
	public string Content { get; set; } = string.Empty;

	/// <summary>
	/// Diff row number
	/// </summary>
	[DataMember(Name = "rowNo")]
	public int RowNo { get; set; }
}
