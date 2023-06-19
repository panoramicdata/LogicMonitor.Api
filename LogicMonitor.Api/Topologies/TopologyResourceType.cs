namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The topology resource type
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TopologyResourceType
{
	/// <summary>
	/// Undiscovered
	/// </summary>
	[EnumMember(Value = "undiscovered")]
	Undiscovered,

	/// <summary>
	/// Device
	/// </summary>
	[EnumMember(Value = "device")]
	Device,

	/// <summary>
	/// Instance
	/// </summary>
	[EnumMember(Value = "instance")]
	Instance,
}