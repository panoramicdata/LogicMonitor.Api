namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The vertex type
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DataVertexType
{
	/// <summary>
	/// Router
	/// </summary>
	[EnumMember(Value = "unknown")]
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
}