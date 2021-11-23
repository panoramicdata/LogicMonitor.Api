namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The DataContract
/// </summary>
[DataContract]
public class TopologyLayout
{
	/// <summary>
	///    The mode
	/// </summary>
	[DataMember(Name = "mode")]
	public string Mode { get; set; }

	/// <summary>
	///    The vertices
	/// </summary>
	[DataMember(Name = "vertices")]
	public List<Vertex> Vertices { get; set; }
}
