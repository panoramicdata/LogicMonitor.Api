namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology Data
/// </summary>
[DataContract]
public class DataVertex : StringIdentifiedItem
{
	/// <summary>
	/// type
	/// </summary>
	[DataMember(Name = "type")]
	public DataVertexType Type { get; set; }

	/// <summary>
	/// managedEdgeTypes
	/// </summary>
	[DataMember(Name = "managedEdgeTypes")]
	public List<ManagedEdgeType> ManagedEdgeTypes { get; set; } = [];

	/// <summary>
	/// Resources
	/// </summary>
	[DataMember(Name = "LMResources")]
	public List<TopologyResource> Resources { get; set; } = [];

	/// <summary>
	/// Alerts
	/// </summary>
	[DataMember(Name = "alerts")]
	public List<TopologyAlert> Alerts { get; set; } = [];
}
