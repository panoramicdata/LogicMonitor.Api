namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Website group
/// </summary>
[DataContract]
public class WebsiteGroup : NamedItem, IHasEndpoint, IHasCustomProperties
{
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
	/// The direct website count
	/// </summary>
	[DataMember(Name = "numOfDirectWebsites")]
	public int DirectWebsiteCount { get; set; }

	/// <summary>
	/// The direct website group count
	/// </summary>
	[DataMember(Name = "numOfDirectSubGroups")]
	public int DirectWebsiteGroupCount { get; set; }

	/// <summary>
	/// Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The full path
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string FullPath { get; set; }

	/// <summary>
	/// The alert group status
	/// </summary>
	[DataMember(Name = "groupStatus")]
	public AlertStatus GroupStatus { get; set; }

	/// <summary>
	/// Whether websites are disabled
	/// </summary>
	[DataMember(Name = "hasWebsitesDisabled")]
	public bool HasWebsitesDisabled { get; set; }

	/// <summary>
	/// Parent Id
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// The alert Status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// The website count
	/// </summary>
	[DataMember(Name = "numOfWebsites")]
	public int WebsiteCount { get; set; }

	/// <summary>
	/// Properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	/// Whether monitoring is stopped
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool? StopMonitoring { get; set; }

	/// <summary>
	/// Child groups
	/// </summary>
	[DataMember(Name = "subGroups")]
	public List<WebsiteGroup> ChildWebsiteGroups { get; set; }

	/// <summary>
	/// The test location - This doesn't seem to be populated with anything useful any more (historical?)
	/// </summary>
	[DataMember(Name = "testLocation")]
	public TestLocation TestLocation { get; set; }

	/// <summary>
	/// User permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermissionString { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "website/groups";
}
