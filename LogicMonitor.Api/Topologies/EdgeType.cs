namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The edge type
/// </summary>
[DataContract]
public class EdgeType
{
	/// <summary>
	/// The type
	/// </summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The type
	/// </summary>
	public EdgeTypeDirection Direction { get; set; }
}