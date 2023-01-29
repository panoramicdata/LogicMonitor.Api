namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Dashboard clone request
/// </summary>
[DataContract]
public class DashboardCloneRequest : CloneRequest<Dashboard>
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The widgetTokens
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<EntityProperty> WidgetTokens { get; set; } = new List<EntityProperty>();

	/// <summary>
	/// Whether it is shareable
	/// </summary>
	[DataMember(Name = "sharable")]
	public bool IsSharable { get; set; } = true;

	///// <summary>
	///// The GroupName
	///// </summary>
	//[DataMember(Name = "groupName")]
	//public string GroupName { get; set; }

	/// <summary>
	/// The GroupId
	/// </summary>
	[DataMember(Name = "groupId")]
	public int DashboardGroupId { get; set; }

	/// <summary>
	/// The WidgetsConfig
	/// </summary>
	[DataMember(Name = "widgetsConfig")]
	public Dictionary<string, WidgetConfig> WidgetsConfig { get; set; }

	/// <summary>
	/// The widgetsOrder
	/// </summary>
	[DataMember(Name = "widgetsOrder")]
	public string WidgetsOrder { get; set; } = string.Empty;
}
