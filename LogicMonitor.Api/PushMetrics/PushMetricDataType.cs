using System.Runtime.Serialization;

namespace LogicMonitor.Api.PushMetrics
{
	/// <summary>
	/// A push metric data type
	/// </summary>
	[DataContract]
	public enum PushMetricDataType
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