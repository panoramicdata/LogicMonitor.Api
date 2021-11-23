namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A push metric datapoint data type
/// </summary>
[DataContract]
public enum PushMetricDataPointDataType
{
	/// <summary>
	/// Gauge
	/// </summary>
	[EnumMember(Value = "guage")]
	Guage,

	/// <summary>
	/// Counter
	/// </summary>
	[EnumMember(Value = "counter")]
	Counter,

	/// <summary>
	/// Derive
	/// </summary>
	[EnumMember(Value = "derive")]
	Derive,
}
