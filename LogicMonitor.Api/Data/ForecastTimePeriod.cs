using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// A forecast time period
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ForecastTimePeriod
	{
		/// <summary>
		/// Unknown time period
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Seven days time period
		/// </summary>
		[EnumMember(Value = "7days")]
		SevenDays = 1,

		/// <summary>
		/// Fourteen days time period
		/// </summary>
		[EnumMember(Value = "14days")]
		FourteenDays = 2,

		/// <summary>
		/// One month time period
		/// </summary>
		[EnumMember(Value = "1month")]
		OneMonth = 3,

		/// <summary>
		/// Three months time period
		/// </summary>
		[EnumMember(Value = "3months")]
		ThreeMonths = 4
	}
}