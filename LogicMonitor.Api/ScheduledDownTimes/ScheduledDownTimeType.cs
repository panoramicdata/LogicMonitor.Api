namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Scheduled down time type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum ScheduledDownTimeType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "")]
	Unknown,

	/// <summary>
	/// Collector
	/// </summary>
	[EnumMember(Value = "CollectorSDT")]
	Collector,

	/// <summary>
	/// Resource
	/// </summary>
	[EnumMember(Value = "ResourceSDT")]
	Resource,

	/// <summary>
	/// Resource Batch Job
	/// </summary>
	[EnumMember(Value = "DeviceBatchJobSDT")]
	ResourceBatchJob,

	/// <summary>
	/// Resource Cluster Alert Def
	/// </summary>
	[EnumMember(Value = "DeviceClusterAlertDefSDT")]
	ResourceClusterAlertDefSdt,

	/// <summary>
	/// Resource Data Source
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceSDT")]
	ResourceDataSource,

	/// <summary>
	/// Resource Data Source Instance Group
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceInstanceGroupSDT")]
	ResourceDataSourceInstanceGroup,

	/// <summary>
	/// Resource Data Source Instance
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceInstanceSDT")]
	ResourceDataSourceInstance,

	/// <summary>
	/// Resource Event Source
	/// </summary>
	[EnumMember(Value = "DeviceEventSourceSDT")]
	ResourceEventSource,

	/// <summary>
	/// Resource Log Pipe Line Resource
	/// </summary>
	[EnumMember(Value = "DeviceLogPipeLineResourceSDT")]
	ResourceLogPipeLineResourceSDT,

	/// <summary>
	/// Resource Group
	/// </summary>
	[EnumMember(Value = "ResourceGroupSDT")]
	ResourceGroup,

	/// <summary>
	/// Service
	/// </summary>
	[EnumMember(Value = "ServiceSDT")]
	Service,
	// Have not created a CreationDto as adding an SDT to a service in the UI appears
	// to use a resourceSDT, so this may not be required

	/// <summary>
	/// Website
	/// </summary>
	[EnumMember(Value = "WebsiteSDT")]
	Website,

	/// <summary>
	/// Website Group
	/// </summary>
	[EnumMember(Value = "WebsiteGroupSDT")]
	WebsiteGroup,

	/// <summary>
	/// Website checkpoint
	/// </summary>
	[EnumMember(Value = "WebsiteCheckpointSDT")]
	WebsiteCheckpoint,
}
