namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The topology layout mode
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TopologyLayoutMode
{
	/// <summary>
	/// Dynamic
	/// </summary>
	[EnumMember(Value = "dynamic")]
	Dynamic,

	/// <summary>
	/// Hierarchical
	/// </summary>
	[EnumMember(Value = "hierarchical")]
	Hierarchical
}