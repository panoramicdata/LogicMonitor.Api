using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     The NocWidgetDisplayColumnCount
	/// </summary>
	public enum NocWidgetDisplayColumnCount
	{
		/// <summary>
		///     Unknown
		/// </summary>
		Unknown = 0,

		/// <summary>
		///     1
		/// </summary>
		[EnumMember(Value = "1")]
		One = 1,

		/// <summary>
		///     2
		/// </summary>
		[EnumMember(Value = "2")]
		Two = 2,

		/// <summary>
		///     3
		/// </summary>
		[EnumMember(Value = "3")]
		Three = 3,

		/// <summary>
		///     4
		/// </summary>
		[EnumMember(Value = "4")]
		Four = 4,

		/// <summary>
		///     5
		/// </summary>
		[EnumMember(Value = "5")]
		Five = 5,

		/// <summary>
		///     6
		/// </summary>
		[EnumMember(Value = "6")]
		Six = 6,

		/// <summary>
		///     7
		/// </summary>
		[EnumMember(Value = "7")]
		Seven = 7
	}
}