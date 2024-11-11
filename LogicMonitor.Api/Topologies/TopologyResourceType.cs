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
	/// Resource
	/// </summary>
	[EnumMember(Value = "device")]
	Resource,

	/// <summary>
	/// Instance
	/// </summary>
	[EnumMember(Value = "instance")]
	Instance,
}