namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A check type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum CheckType
{
	/// <summary>
	/// Synthetics
	/// </summary>
	[EnumMember(Value = "synthetics")]
	Synthetics = 0,
}