namespace LogicMonitor.Api.Topologies;

/// <summary>
///    A device group
/// </summary>
[DataContract]
public class TopologyGroup : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	///    The topology count
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "topologiesCount")]
	public int TopologiesCount { get; set; }

	/// <summary>
	///    The Group status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "topology/groups";

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>FullPath</returns>
	public override string ToString() => Name;
}
