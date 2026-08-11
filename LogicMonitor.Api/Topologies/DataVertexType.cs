namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The vertex type
/// </summary>
/// <remarks>
/// LogicMonitor adds vertex types over time (e.g. "Wireless"), and the topology data endpoint
/// returns whatever the TopologySources produced. A strict converter therefore fails the whole
/// /topology/data call on the first unmodelled vertex, so this enum uses the tolerant converter
/// and degrades an unrecognised type to <see cref="Unknown"/> instead.
/// </remarks>
[JsonConverter(typeof(TolerantStringEnumConverter))]
public enum DataVertexType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "Unknown")]
	[EnumMemberAlias("unknown", "UNKNOWN")]
	Unknown,

	/// <summary>
	/// VMDatastore
	/// </summary>
	[EnumMember(Value = "Undiscovered")]
	Undiscovered,

	/// <summary>
	/// VMDatastore
	/// </summary>
	[EnumMember(Value = "VMDatastore")]
	VmDatastore,

	/// <summary>
	/// VirtualMachine
	/// </summary>
	[EnumMember(Value = "VirtualMachine")]
	VirtualMachine,

	/// <summary>
	/// VirtualMachineResource
	/// </summary>
	[EnumMember(Value = "VirtualMachineResource")]
	VirtualMachineResource,

	/// <summary>
	/// HyperVisor
	/// </summary>
	[EnumMember(Value = "HyperVisor")]
	Hypervisor,

	/// <summary>
	/// HyperVisor cluster
	/// </summary>
	[EnumMember(Value = "HyperVisorCluster")]
	HypervisorCluster,

	/// <summary>
	/// Switch
	/// </summary>
	[EnumMember(Value = "Switch")]
	Switch,

	/// <summary>
	/// Router
	/// </summary>
	[EnumMember(Value = "Router")]
	Router,

	/// <summary>
	/// Firewall
	/// </summary>
	[EnumMember(Value = "Firewall")]
	Firewall,

	/// <summary>
	/// Service
	/// </summary>
	[EnumMember(Value = "Service")]
	Service,

	/// <summary>
	/// PhysicalServer
	/// </summary>
	[EnumMember(Value = "PhysicalServer")]
	PhysicalServer,

	/// <summary>
	/// AccessPoint
	/// </summary>
	[EnumMember(Value = "AccessPoint")]
	AccessPoint,

	/// <summary>
	/// Wireless (e.g. a wireless access point or controller)
	/// </summary>
	[EnumMember(Value = "Wireless")]
	Wireless,

	/// <summary>
	/// LoadBalancer
	/// </summary>
	[EnumMember(Value = "LoadBalancer")]
	LoadBalancer,

	/// <summary>
	/// StorageNode
	/// </summary>
	[EnumMember(Value = "StorageNode")]
	StorageNode,

	/// <summary>
	/// Cluster
	/// </summary>
	[EnumMember(Value = "Cluster")]
	Cluster,

	/// <summary>
	/// PhysicalDisk
	/// </summary>
	[EnumMember(Value = "PhysicalDisk")]
	PhysicalDisk,

	/// <summary>
	/// StorageVolume
	/// </summary>
	[EnumMember(Value = "StorageVolume")]
	StorageVolume,

	/// <summary>
	/// VirtualDisk
	/// </summary>
	[EnumMember(Value = "VirtualDisk")]
	VirtualDisk,
}