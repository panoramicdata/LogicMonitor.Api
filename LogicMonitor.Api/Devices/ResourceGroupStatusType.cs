namespace LogicMonitor.Api.Devices;

/// <summary>
/// The Group Status Type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum ResourceGroupStatusType
{
	/// <summary>
	/// Normal
	/// </summary>
	[EnumMember(Value = "normal")]
	Normal = 0,

	/// <summary>
	/// Dead
	/// </summary>
	[EnumMember(Value = "dead")]
	Dead = 1,

	/// <summary>
	/// Dead
	/// </summary>
	[EnumMember(Value = "dead-collector")]
	DeadCollector = 2
}