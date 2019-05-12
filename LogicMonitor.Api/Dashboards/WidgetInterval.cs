using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///    A widget refresh interval
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum WidgetInterval
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		///    1 minutes
		/// </summary>
		[EnumMember(Value = "1")]
		OneMinute,

		/// <summary>
		///    3 minutes
		/// </summary>
		[EnumMember(Value = "3")]
		ThreeMinutes,

		/// <summary>
		///    5 minutes
		/// </summary>
		[EnumMember(Value = "5")]
		FiveMinutes,

		/// <summary>
		///    10 minutes
		/// </summary>
		[EnumMember(Value = "10")]
		TenMinutes,

		/// <summary>
		///    15 minutes
		/// </summary>
		[EnumMember(Value = "15")]
		FifteenMinutes
	}
}