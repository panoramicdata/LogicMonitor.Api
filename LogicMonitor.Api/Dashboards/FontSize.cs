using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// Font size
	/// </summary>
	[DataContract]
	public enum FontSize
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Small
		/// </summary>
		[EnumMember(Value = "smaller")]
		Smaller,

		/// <summary>
		/// Small
		/// </summary>
		[EnumMember(Value = "small-font")]
		Small,

		/// <summary>
		/// Normal
		/// </summary>
		[EnumMember(Value = "normal-font")]
		Normal,

		/// <summary>
		/// Large
		/// </summary>
		[EnumMember(Value = "large-font")]
		Large,

		/// <summary>
		/// Extra large
		/// </summary>
		[EnumMember(Value = "x-large-font")]
		ExtraLarge,
	}
}