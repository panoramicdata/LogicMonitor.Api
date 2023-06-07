namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A dashboard
/// </summary>
[DataContract]
public class DashboardCreationResponse
	: NamedItem,
	IPatchable,
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
	public string FullName { get; set; } = string.Empty;

	/// <summary>
	///     The dashboard group Id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int DashboardGroupId { get; set; }

	/// <summary>
	///     The dashboard group Id
	/// </summary>
	[DataMember(Name = "groupName")]
	public string DashboardGroupName { get; set; } = string.Empty;

	/// <summary>
	///     The dashboard group full path
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string DashboardGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The dashboard owner
	/// </summary>
	[DataMember(Name = "owner")]
	public string Owner { get; set; } = string.Empty;

	/// <summary>
	/// The template which is used for importing dashboard
	/// </summary>
	[DataMember(Name = "template")]
	public object Template { get; set; } = new();

	/// <summary>
	///     Whether to use widget tokens
	/// </summary>
	[DataMember(Name = "useDynamicWidget")]
	public bool UseWidgetTokens { get; set; }

	/// <summary>
	///     Whether to use widget tokens
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public string[] WidgetTokens { get; set; } = Array.Empty<string>();

	/// <summary>
	/// Overwrite existing Resource/Website Group fields with ##defaultResourceGroup## and/or ##defaultWebsiteGroup## tokens. This value of this attribute is only considered while updating the Dashboard configuration. While creating the new Dashboard, this value will always be considered as false irrespective of the passed value.
	/// </summary>
	[DataMember(Name = "overwriteGroupFields")]
	public bool OverwriteGroupFields { get; set; }

	/// <summary>
	///     The widgets configuration
	/// </summary>
	[DataMember(Name = "widgetsConfig")]
	public object WidgetsConfig { get; set; } = new();

	/// <summary>
	///     The order of the widgets
	/// </summary>
	[DataMember(Name = "widgetsOrder")]
	public string WidgetsOrder { get; set; } = string.Empty;

	/// <summary>
	///     The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "dashboard/dashboards";
}
