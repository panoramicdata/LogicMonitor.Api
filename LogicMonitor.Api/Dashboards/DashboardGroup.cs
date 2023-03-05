namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A dashboard group
/// </summary>
[DataContract]
public class DashboardGroup : NamedItem, IPatchable, IHasCustomProperties
{
	/// <summary>
	///    The full path
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string FullPath { get; set; } = string.Empty;

	/// <summary>
	///    The parent dashboard group id
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	///    The current user's access permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///    The number of direct child dashboard groups
	/// </summary>
	[DataMember(Name = "numOfDirectSubGroups")]
	public int DirectSubGroupCount { get; set; }

	/// <summary>
	///    The total number of child dashboards, including those in subgroups
	/// </summary>
	[DataMember(Name = "numOfDashboards")]
	public int DashboardCount { get; set; }

	/// <summary>
	///    The total number of child dashboards, not including those in subgroups
	/// </summary>
	[DataMember(Name = "numOfDirectDashboards")]
	public int DirectDashboardCount { get; set; }

	/// <summary>
	///    The total number of child dashboards, not including those in subgroups
	/// </summary>
	[DataMember(Name = "dashboards")]
	public List<Dashboard> Dashboards { get; set; } = new();

	/// <summary>
	/// The template which is used for importing dashboard group
	/// </summary>
	[DataMember(Name = "template")]
	public object Template { get; set; } = new();

	/// <summary>
	///    The widget tokens
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<EntityProperty> CustomProperties { get; set; } = new();

	/// <inheritdoc />
	public string Endpoint() => "dashboard/groups";
}
