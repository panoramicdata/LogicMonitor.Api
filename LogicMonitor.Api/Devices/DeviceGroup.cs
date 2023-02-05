namespace LogicMonitor.Api.Devices;

/// <summary>
///    A device group
/// </summary>
[DataContract]
public class DeviceGroup : NamedItem, IHasCustomProperties, IPatchable
{
	/// <summary>
	///    The Alert disable status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///    Whether alerting is enabled
	/// </summary>
	[Obsolete("Use !IsAlertingDisabled instead", true)]
	public bool AlertEnable => !IsAlertingDisabled;

	/// <summary>
	///    The alert status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertStatus", IsRequired = false)]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The Applies to custom query for this group (only for dynamic groups)
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; }

	/// <summary>
	///    The auto visual result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "autoVisualResult")]
	public string AutoVisualResult { get; set; }

	/// <summary>
	///    The clusterAlertStatus
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "clusterAlertStatus")]
	public string ClusterAlertStatus { get; set; }

	/// <summary>
	///    The cluster alert status priority
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "clusterAlertStatusPriority")]
	public int ClusterAlertStatusPriority { get; set; }

	/// <summary>
	/// The id of the default Auto Balanced Collector Group assigned to the device group
	/// </summary>
	[DataMember(Name = "defaultAutoBalancedCollectorGroupId")]
	public int DefaultAutoBalancedCollectorGroupId { get; set; }

	/// <summary>
	/// The description of the default collector assigned to the device group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "defaultCollectorDescription")]
	public string DefaultCollectorDescription { get; set; }

	/// <summary>
	/// The Id of the default collector assigned to the device group
	/// </summary>
	[DataMember(Name = "defaultCollectorId", IsRequired = false)]
	public int DefaultCollectorId { get; set; }

	/// <summary>
	///    The default Collector Id (used by MonitorObjectGroups only)
	/// </summary>
	[DataMember(Name = "defaultAgentId", IsRequired = false)]
	public int DefaultAgentId { get; set; }

	/// <summary>
	/// The number of AWS devices that belong to this device group (includes AWS devices in sub groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfAWSDevices")]
	public int AwsDeviceCount { get; set; }

	/// <summary>
	/// The number of instances in each AWS region (only applies to AWS groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsRegionsInfo")]
	public string AwsRegionsInfo { get; set; }

	/// <summary>
	/// The String result returned by the transaction that tests the AWS credentials associated with the AWS group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsTestResult")]
	public AwsAccountTestResult? AwsTestResult { get; set; }

	/// <summary>
	/// The Status code result returned by the transaction that tests the AWS credentials associated with the AWS group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsTestResultCode")]
	public int AwsTestResultCode { get; set; }

	/// <summary>
	/// The number of Azure devices that belong to this device group (includes Azure devices in sub groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfAzureDevices")]
	public int AzureDeviceCount { get; set; }

	/// <summary>
	/// The number of instances in each Azure region (only applies to Azure groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureRegionsInfo")]
	public string AzureRegionsInfo { get; set; }

	/// <summary>
	/// The String result returned by the transaction that tests the Azure credentials associated with the Azure group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureTestResult")]
	public string AzureTestResult { get; set; }

	/// <summary>
	/// The Status code result returned by the transaction that tests the Azure credentials associated with the Azure group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureTestResultCode")]
	public long AzureTestResultCode { get; set; }

	/// <summary>
	///    GCP Device count
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfGcpDevices")]
	public int GcpDeviceCount { get; set; }

	/// <summary>
	///    GCP regions info
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpRegionsInfo")]
	public string GcpRegionsInfo { get; set; }

	/// <summary>
	/// The result returned by the transaction that tests the GCP credentials associated with the GCP group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpTestResult")]
	public string GcpTestResult { get; set; }

	/// <summary>
	/// The Status code result returned by the transaction that tests the GCP credentials associated with the GCP group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpTestResultCode")]
	public int GcpTestResultCode { get; set; }

	/// <summary>
	/// Indicates whether Netflow is enabled (true) or disabled (false) for the device group, the default value is true
	/// </summary>
	[DataMember(Name = "enableNetflow")]
	public bool IsNetflowEnabled { get; set; }

	/// <summary>
	/// Whether if any Netflow enabled devices in this device group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "hasNetflowEnabledDevices")]
	public bool HasNetflowEnabledDevices { get; set; }

	/// <summary>
	/// The time, in epoch seconds format, that the device group was created
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdOn")]
	public int? CreatedOnTimestampUtc { get; set; }

	/// <summary>
	///    The UTC DateTime that the device was created in the system
	/// </summary>
	[IgnoreDataMember]
	public DateTime? CreatedOnUtc => CreatedOnTimestampUtc?.ToNullableDateTimeUtc();

	/// <summary>
	/// The properties associated with this device group
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; }

	/// <summary>
	/// The number of total devices, including both AWS and normal devices, that belong to this device group (includes normal devices in sub groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfHosts")]
	public long DeviceCount { get; set; }

	/// <summary>
	/// The collector group id of the default collector assigned to the device group
	/// </summary>
	[DataMember(Name = "defaultCollectorGroupId")]
	public int DefaultCollectorGroupId { get; set; }

	/// <summary>
	/// The description of the default collector group assigned to the device group
	/// </summary>
	[DataMember(Name = "defaultCollectorGroupDescription")]
	public string DefaultCollectorGroupDescription { get; set; }

	/// <summary>
	///    The default load balance collector group id
	/// </summary>
	[DataMember(Name = "defaultLoadBalanceCollectorGroupId")]
	public int DefaultLoadBalanceCollectorGroupId { get; set; }

	/// <summary>
	/// The type of device group: normal and dynamic device groups will have groupType\u003dNormal, and AWS groups will have a groupType value of AWS/SERVICE (e.g. AWS/S3)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "groupType")]
	public DeviceGroupType DeviceGroupType { get; set; }

	/// <summary>
	///    The Devices in this DeviceGroup.
	///    This information does not always come from the API, so you may need to populate it yourself.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(IsRequired = false)]
	public List<Device> Devices { get; set; }

	/// <summary>
	/// The number of AWS and normal devices that belong only to this device group (doesn\u0027t include devices in sub-groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfDirectDevices")]
	public long DirectDeviceCount { get; set; }

	/// <summary>
	/// The number of sub-groups that belong only to this device group (doesn\u0027t include groups under sub-groups)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfDirectSubGroups")]
	public long DirectSubGroupCount { get; set; }

	/// <summary>
	///    The alert status Priority
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Whether or not alerting is effectively disabled for this device group (alerting may be disabled at a higher level, e.g. parent group)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "effectiveAlertEnabled")]
	public bool EffectiveAlertEnabled { get; set; }

	/// <summary>
	/// The extra setting for cloud group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "extra")]
	public object? Extra { get; set; }

	/// <summary>
	/// The full path of the device group (i.e. if the group \u0027Dev\u0027 is under a parent group named \u0027Production\u0027, the fullPath would be \u0027Production/Dev\u0027
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "fullPath")]
	public string FullPath { get; set; }

	/// <summary>
	/// normal | dead \nThe status of this device group, where possible statuses are normal and dead. A group with a status of dead may indicate that one or more devices are dead within the group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "groupStatus")]
	public string GroupStatus { get; set; }

	/// <summary>
	/// Indicates whether alerting is disabled (true) or enabled (false) for this device group
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool IsAlertingDisabled { get; set; }

	/// <summary>
	/// The number of kubernetes devices that belong to this device group (includes Kubernetes devices in sub groups)
	/// </summary>
	[DataMember(Name = "numOfKubernetesDevices")]
	public int KubernetesDeviceCount { get; set; }

	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertingDisabledOn")]
	public object? AlertingDisabledOn { get; set; }

	/// <summary>
	/// The id of the parent group for this device group (the root device group has an Id of 1)
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// The role privilege operations for the device group that are granted to the user that made this API request
	/// </summary>
	[DataMember(Name = "rolePrivileges")]
	public List<RolePrivilege> RolePrivileges { get; set; }

	/// <summary>
	/// The result returned by the transaction that tests the SaaS credentials associated with the Saas group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "saasTestResult")]
	public string SaasTestResult { get; set; }

	/// <summary>
	/// The Status code result returned by the transaction that tests the SaaS credentials associated with the SaaS group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "saasTestResultCode")]
	public int SaasTestResultCode { get; set; }

	/// <summary>
	///    The SDT status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// The child device groups within this device group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "subGroups")]
	public List<DeviceGroup> SubGroups { get; set; }

	/// <summary>
	/// The permissions for the device group that are granted to the user that made this API request
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "device/groups";

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>FullPath</returns>
	public override string ToString() => FullPath;
}
