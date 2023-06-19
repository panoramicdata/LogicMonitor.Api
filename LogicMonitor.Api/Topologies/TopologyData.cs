namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology Data
/// </summary>
[DataContract]
public class TopologyData
{
	/// <summary>
	/// vertices
	/// </summary>
	[DataMember(Name = "vertices")]
	public List<DataVertex> Vertices { get; set; } = new();

	/// <summary>
	/// neighbourVertices
	/// </summary>
	[DataMember(Name = "neighbourVertices")]
	public object? NeighbourVertices { get; set; }

	/// <summary>
	/// XXXXXXX
	/// </summary>
	[DataMember(Name = "edges")]
	public List<Edge> Edges { get; set; } = new();
}
