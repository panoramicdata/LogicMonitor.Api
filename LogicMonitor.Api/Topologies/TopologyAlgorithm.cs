namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The topology algorithm
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TopologyAlgorithm
{
	/// <summary>
	/// Dynamic
	/// </summary>
	[EnumMember(Value = "I-Feel-Lucky")]
	Dynamic,

	/// <summary>
	/// 1st degree away
	/// </summary>
	[EnumMember(Value = "Neighbours-1")]
	FirstDegreeAway,

	/// <summary>
	/// I Feel Lucky
	/// </summary>
	[EnumMember(Value = "Neighbours-2")]
	SecondDegreeAway,
}