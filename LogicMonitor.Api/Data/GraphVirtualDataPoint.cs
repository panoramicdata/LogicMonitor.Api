namespace LogicMonitor.Api.Data;

/// <summary>
/// Graph virtual data point
/// </summary>

[DataContract]
public class GraphVirtualDataPoint : UndescribedNamedItem
{
	/// <summary>
	/// the graph virtual data point rpn expression
	/// </summary>
	[DataMember(Name = "rpn")]
	public string? Rpn { get; set; }
}
