namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Website group
/// </summary>
[DataContract]
public class WebsiteGroup : NamedItem, IHasEndpoint, IHasCustomProperties
{

	/// <summary>
	/// The full path of the group
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string? FullPath { get; set; }

	/// <summary>
	/// true: monitoring is disabled for the websites in the group\nfalse: monitoring is enabled for the websites in the group\nIf stopMonitoring\u003dtrue, then alerting will also by default be disabled for the websites in the group
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The permission level of the user that made the API request. Acceptable values are: write, read, ack
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermissionString { get; set; }

	/// <summary>
	/// An object that indicates the websites locations.\ne.g. {\u0027all\u0027: false, smgId:[1,2,3], collectorIds:[14,16]}
	/// </summary>
	[DataMember(Name = "testLocation")]
	public WebsiteLocation? TestLocation { get; set; }

	/// <summary>
	/// Indicates if there are websites disabled in this group
	/// </summary>
	[DataMember(Name = "hasWebsitesDisabled")]
	public bool HasWebsitesDisabled { get; set; }

	/// <summary>
	/// true: alerting is disabled for the websites in the group\nfalse: alerting is enabled for the websites in the group\nIf stopMonitoring\u003dtrue, then alerting will also by default be disabled for the websites in the group
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The privilege operations of the user\u0027s role that made the API request.  The array can contain the values ack, sdt and/or threshold
	/// </summary>
	[DataMember(Name = "rolePrivileges")]
	public RolePrivilege[]? RolePrivileges { get; set; }

	/// <summary>
	/// The Id of the parent group. If parentId\u003d1 then the group exists under the root  group
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// The number of direct websites in this group
	/// </summary>
	[DataMember(Name = "numOfDirectWebsites")]
	public int DirectWebsiteCount { get; set; }

	/// <summary>
	/// The number of direct website groups in this group (excluding those in subgroups)
	/// </summary>
	[DataMember(Name = "numOfDirectSubGroups")]
	public int DirectWebsiteGroupCount { get; set; }

	/// <summary>
	/// The number of websites in the service group, including the websites in sub groups
	/// </summary>
	[DataMember(Name = "numOfWebsites")]
	public int WebsiteCount { get; set; }

	/// <summary>
	/// Properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<EntityProperty>? CustomProperties { get; set; }

	/// <summary>
	/// The alert Status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The alert status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Alert disable status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The alert group status
	/// </summary>
	[DataMember(Name = "groupStatus")]
	public WebsiteGroupStatusType GroupStatus { get; set; }

	/// <summary>
	/// The alert Status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// Child groups
	/// </summary>
	[DataMember(Name = "subGroups")]
	public List<WebsiteGroup> ChildWebsiteGroups { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "website/groups";
}
