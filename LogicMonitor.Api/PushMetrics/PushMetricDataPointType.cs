using System.Runtime.Serialization;

namespace LogicMonitor.Api.PushMetrics
{
	/// <summary>
	/// A push metric data point type
	/// </summary>
	[DataContract]
	public enum PushMetricDataPointType
	{
		/// <summary>
		/// Guage
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
}