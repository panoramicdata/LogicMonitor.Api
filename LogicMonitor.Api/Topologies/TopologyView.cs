namespace LogicMonitor.Api.Topologies;

/// <summary>
/// A Topology View
/// </summary>
[DataContract]
public class TopologyView
{
	/// <summary>
	///    The resource id
	/// </summary>
	[DataMember(Name = "rid")]
	public string ResourceId { get; set; } = string.Empty;

	/// <summary>
	///    The resource
	/// </summary>
	[DataMember(Name = "resource")]
	public string Resource { get; set; } = string.Empty;

	/// <summary>
	///    The vertices
	/// </summary>
	[DataMember(Name = "vertices")]
	public List<string>? Vertices { get; set; } = [];

	/// <summary>
	///    The edge types
	/// </summary>
	[DataMember(Name = "edgeTypes")]
	public List<EdgeTypeAndDirection> EdgeTypes { get; set; } = [];

	/// <summary>
	///    The algorithm
	/// </summary>
	[DataMember(Name = "algorithm")]
	public TopologyAlgorithm Algorithm { get; set; }
}
