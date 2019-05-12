using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Time
{
	/// <summary>
	///     A discrete time period, as supported by the LogicMonitor API
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TimePeriod
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		///     Zoom
		/// </summary>
		[EnumMember(Value = "zoom")]
		Zoom = 1,

		/// <summary>
		///     One hour
		/// </summary>
		[EnumMember(Value = "1hour")]
		OneHour = 2,

		/// <summary>
		///     Two hours
		/// </summary>
		[EnumMember(Value = "2hour")]
		TwoHours = 3,

		/// <summary>
		///     Five hours
		/// </summary>
		[EnumMember(Value = "5hour")]
		FiveHours = 4,

		/// <summary>
		///     One day
		/// </summary>
		[EnumMember(Value = "1day")]
		OneDay = 5,

		/// <summary>
		///     Two days
		/// </summary>
		[EnumMember(Value = "2days")]
		TwoDays = 6,

		/// <summary>
		///     Seven Days
		/// </summary>
		[EnumMember(Value = "7days")]
		SevenDays = 7,

		/// <summary>
		///     One month
		/// </summary>
		[EnumMember(Value = "1month")]
		OneMonth = 8,

		/// <summary>
		///     One month
		/// </summary>
		[EnumMember(Value = "3month")]
		ThreeMonths = 9,

		/// <summary>
		///     One year
		/// </summary>
		[EnumMember(Value = "1year")]
		OneYear = 10
	}
}