namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology Data
/// </summary>
[DataContract]
public class ManagedEdgeType
{

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Direction
	/// </summary>
	[DataMember(Name = "direction")]
	public EdgeDirection Direction { get; set; }

	/// <summary>
	/// Count
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }
}
