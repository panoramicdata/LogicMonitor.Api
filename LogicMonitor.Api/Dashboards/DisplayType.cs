using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// Display type
	/// </summary>
	[DataContract]
	public enum DisplayType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Line
		/// </summary>
		[EnumMember(Value = "line")]
		Line = 1,

		/// <summary>
		/// Stack
		/// </summary>
		[EnumMember(Value = "stack")]
		Stack,

		/// <summary>
		/// Stack
		/// </summary>
		[EnumMember(Value = "area")]
		Area,

		/// <summary>
		/// Raw
		/// </summary>
		[EnumMember(Value = "raw")]
		Raw,

		/// <summary>
		/// Percent
		/// </summary>
		[EnumMember(Value = "percent")]
		Percent,

		/// <summary>
		/// Column
		/// </summary>
		[EnumMember(Value = "column")]
		Column,

		/// <summary>
		/// Color bar
		/// </summary>
		[EnumMember(Value = "colorBar")]
		ColorBar,
	}
}