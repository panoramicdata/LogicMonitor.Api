using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// Widget timescale
	/// </summary>
	[DataContract]
	public enum WidgetTimescale
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[DataMember(Name = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Day
		/// </summary>
		[DataMember(Name = "day")]
		Day = 1,

		/// <summary>
		/// OneDay
		/// </summary>
		[DataMember(Name = "1day")]
		OneDay = 2,

		/// <summary>
		/// TwoDays
		/// </summary>
		[DataMember(Name = "2days")]
		TwoDays = 3,

		/// <summary>
		/// SevenDays
		/// </summary>
		[DataMember(Name = "7days")]
		SevenDays = 4,

		/// <summary>
		/// Week
		/// </summary>
		[DataMember(Name = "week")]
		Week = 5,

		/// <summary>
		/// OneHour
		/// </summary>
		[DataMember(Name = "1hour")]
		OneHour = 6,

		/// <summary>
		/// TwoHour
		/// </summary>
		[DataMember(Name = "2hour")]
		TwoHour = 7,

		/// <summary>
		/// FiveHour
		/// </summary>
		[DataMember(Name = "5hour")]
		FiveHour = 8,
	}
}