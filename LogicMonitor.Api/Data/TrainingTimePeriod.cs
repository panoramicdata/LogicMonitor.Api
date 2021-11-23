namespace LogicMonitor.Api.Data;

/// <summary>
/// A training time period
/// </summary>
[DataContract]
public enum TrainingTimePeriod
{
	/// <summary>
	/// Unknown time period
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// One month time period
	/// </summary>
	[EnumMember(Value = "1month")]
	OneMonth = 1,

	/// <summary>
	/// Three months time period
	/// </summary>
	[EnumMember(Value = "3months")]
	ThreeMonths = 2,

	/// <summary>
	/// Six months time period
	/// </summary>
	[EnumMember(Value = "6months")]
	SixMonths = 3,

	/// <summary>
	/// One year time period
	/// </summary>
	[EnumMember(Value = "1year")]
	OneYear = 4
}
