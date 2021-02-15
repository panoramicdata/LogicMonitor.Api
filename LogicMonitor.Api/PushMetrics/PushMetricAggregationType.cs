using System.Runtime.Serialization;

namespace LogicMonitor.Api.PushMetrics
{
	/// <summary>
	/// A push metric aggregation type
	/// </summary>
	[DataContract]
	public enum PushMetricAggregationType
	{
		/// <summary>
		/// None
		/// </summary>
		[EnumMember(Value = "none")]
		None,

		/// <summary>
		/// Mean
		/// </summary>
		[EnumMember(Value = "avg")]
		Mean,

		/// <summary>
		/// Sum
		/// </summary>
		[EnumMember(Value = "sum")]
		Sum,
	}
}