namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A Resource DataSource Instance Group
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceGroup : NamedItem
{
	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; } = new();

	/// <summary>
	/// The alert status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The alert disable status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The alert status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// time when the group was created
	/// </summary>
	[DataMember(Name = "createOn")]
	public long CreatedOnUtcSeconds { get; set; }

	/// <summary>
	/// The id of associated Resource
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The display name of the Resource
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource datasource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	///    groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; } = [];

	/// <summary>
	///    Instance count
	/// </summary>
	[DataMember(Name = "instancesNum")]
	public int InstanceCount { get; set; }

	/// <summary>
	///    Disabled Instance count
	/// </summary>
	[DataMember(Name = "disabledInstancesNum")]
	public int DisabledInstanceCount { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; } = string.Empty;

	/// <summary>
	/// The SDT status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }
}
