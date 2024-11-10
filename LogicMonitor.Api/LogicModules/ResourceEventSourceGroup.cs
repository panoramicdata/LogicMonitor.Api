namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    A Resource EventSource Group
/// </summary>
[DataContract]
public class ResourceEventSourceGroup : NamedItem
{
	/// <summary>
	///    Resource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	///    Resource DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    createOn
	/// </summary>
	[DataMember(Name = "createOn")]
	public long CreatedOnSeconds { get; set; }

	/// <summary>
	///    createOn
	/// </summary>
	[IgnoreDataMember]
	public DateTime CreatedOnDateTimeUtc => CreatedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    alertStatus
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	///    alertStatusPriority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	///    sdtStatus
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	///    alertDisableStatus
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///    sdtAt
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; } = string.Empty;

	/// <summary>
	///    AlertingDisabledOn
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; } = new();

	/// <summary>
	///    groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; } = [];

	/// <summary>
	///    instancesNum
	/// </summary>
	[DataMember(Name = "instancesNum")]
	public int InstanceCount { get; set; }

	/// <summary>
	///    disabledInstancesNum
	/// </summary>
	[DataMember(Name = "disabledInstancesNum")]
	public int DisabledInstanceCount { get; set; }

	/// <summary>
	///    Resource EventSourceId
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int ResourceEventSourceId { get; set; }
}
