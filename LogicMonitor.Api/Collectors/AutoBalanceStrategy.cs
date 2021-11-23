namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AutoBalanceStrategy
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum AutoBalanceStrategy
{
	/// <summary>
	/// None
	/// </summary>
	[EnumMember(Value = "none")]
	None
}
