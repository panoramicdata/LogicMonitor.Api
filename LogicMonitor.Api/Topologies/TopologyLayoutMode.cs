namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The topology layout mode
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TopologyLayoutMode
{
	/// <summary>
	/// Circular
	/// </summary>
	[EnumMember(Value = "CIRCULAR")]
	Circular,

	/// <summary>
	/// Hierarchical
	/// </summary>
	[EnumMember(Value = "HIERARCHIC")]
	Hierarchical,

	/// <summary>
	/// Horizontal
	/// </summary>
	[EnumMember(Value = "HORIZONTAL")]
	Horizontal,
}