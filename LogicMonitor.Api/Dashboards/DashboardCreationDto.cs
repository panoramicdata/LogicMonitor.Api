namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A dashboard creation dto
/// </summary>
[DataContract]
public class DashboardCreationDto : CreationDto<Dashboard>
{
	/// <summary>
	/// Owner
	/// </summary>
	[DataMember(Name = "owner")]
	public string Owner { get; set; } = string.Empty;

	/// <summary>
	/// The template which is used for importing dashboard
	/// </summary>
	[DataMember(Name = "template")]
	public object Template { get; set; } = new();

	/// <summary>
	/// The id of the group the dashboard belongs to
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The description of the dashboard
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the dashboard is sharable. This value will always be true unless the dashboard is a private dashboard
	/// </summary>
	[DataMember(Name = "sharable")]
	public bool Sharable { get; set; }

	/// <summary>
	/// Information about widget configuration used by the UI
	/// </summary>
	[DataMember(Name = "widgetsConfig")]
	public Dictionary<string, WidgetConfig> WidgetsConfig { get; set; } = [];

	/// <summary>
	/// The name of group where created dashboard will reside
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// If useDynamicWidget\u003dtrue, this field must at least contain tokens defaultDeviceGroup and defaultServiceGroup
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<string> WidgetTokens { get; set; } = [];

	/// <summary>
	/// The name of the dashboard
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
