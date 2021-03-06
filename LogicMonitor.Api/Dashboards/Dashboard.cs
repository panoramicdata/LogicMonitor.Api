using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     A dashboard
	/// </summary>
	[DataContract]
	public class Dashboard
		: NamedItem,
		IPatchable,
		IHasCustomProperties,
		ICloneableItem
	{
		/// <summary>
		///     Whether the dashboard is shareable
		/// </summary>
		[DataMember(Name = "sharable")]
		public bool Sharable { get; set; }

		/// <summary>
		///     The administration Id
		/// </summary>
		[DataMember(Name = "adminId")]
		public int AdminId { get; set; }

		/// <summary>
		///     The full name
		/// </summary>
		[DataMember(Name = "fullName")]
		public string FullName { get; set; }

		/// <summary>
		///     The dashboard group Id
		/// </summary>
		[DataMember(Name = "groupId")]
		public int DashboardGroupId { get; set; }

		/// <summary>
		///     The dashboard group Id
		/// </summary>
		[DataMember(Name = "groupName")]
		public string DashboardGroupName { get; set; }

		/// <summary>
		///     The dashboard group full path
		/// </summary>
		[DataMember(Name = "groupFullPath")]
		public string DashboardGroupFullPath { get; set; }

		/// <summary>
		///     The dashboard owner
		/// </summary>
		[DataMember(Name = "owner")]
		public string Owner { get; set; }

		/// <summary>
		///     The template
		/// </summary>
		[DataMember(Name = "template")]
		public string Template { get; set; }

		/// <summary>
		///     Whether to use widget tokens
		/// </summary>
		[DataMember(Name = "useDynamicWidget")]
		public bool UseWidgetTokens { get; set; }

		/// <summary>
		///     Whether to use widget tokens
		/// </summary>
		[DataMember(Name = "widgetTokens")]
		public List<Property> CustomProperties { get; set; }

		/// <summary>
		///     The widgets configuration
		/// </summary>
		[DataMember(Name = "widgetsConfig")]
		public Dictionary<string, WidgetConfig> WidgetsConfig { get; set; }

		/// <summary>
		///     The order of the widgets
		/// </summary>
		[DataMember(Name = "widgetsOrder")]
		public string WidgetsOrder { get; set; }

		/// <summary>
		///     The user permission
		/// </summary>
		[DataMember(Name = "userPermission")]
		public UserPermission UserPermission { get; set; }

		/// <inheritdoc />
		public string Endpoint() => "dashboard/dashboards";
	}
}