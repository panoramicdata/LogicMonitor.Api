namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Edge
/// </summary>
[DataContract]
public class Edge
{
	/// <summary>
	/// from
	/// </summary>
	[DataMember(Name = "from")]
	public string From { get; set; } = string.Empty;

	/// <summary>
	/// to
	/// </summary>
	[DataMember(Name = "to")]
	public string To { get; set; } = string.Empty;

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// subType
	/// </summary>
	[DataMember(Name = "subType")]
	public string SubType { get; set; } = string.Empty;
}
