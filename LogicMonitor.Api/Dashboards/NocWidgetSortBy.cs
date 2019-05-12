using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// The Noc Widget Sort by
	/// </summary>
	public enum NocWidgetSortBy
	{
		/// <summary>
		/// Unknown type
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Sort by group
		/// </summary>
		[EnumMember(Value = "group")]
		Group = 1,

		/// <summary>
		/// Sort by Level
		/// </summary>
		[EnumMember(Value = "level")]
		Level = 2,

		/// <summary>
		/// Sort by name
		/// </summary>
		[EnumMember(Value = "name")]
		Name = 3,

		/// <summary>
		/// Sort by alert severity
		/// </summary>
		[EnumMember(Value = "alertSeverity")]
		AlertSeverity = 4
	}
}