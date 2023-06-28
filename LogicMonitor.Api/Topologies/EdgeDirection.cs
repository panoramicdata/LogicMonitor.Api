namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The edge type direction
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum EdgeDirection
{
	/// <summary>
	/// In
	/// </summary>
	[EnumMember(Value = "in")]
	In,

	/// <summary>
	/// Out
	/// </summary>
	[EnumMember(Value = "out")]
	Out
}
