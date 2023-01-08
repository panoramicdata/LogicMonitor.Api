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
	/// Device Batch Job
	/// </summary>
	[EnumMember(Value = "DeviceBatchJobSDT")]
	DeviceBatchJob,

	/// <summary>
	/// Device Cluster Alert Def
	/// </summary>
	[EnumMember(Value = "DeviceClusterAlertDefSDT")]
	DeviceClusterAlertDefSdt,

	/// <summary>
	/// Device Data Source
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceSDT")]
	DeviceDataSource,

	/// <summary>
	/// Device Data Source Instance Group
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceInstanceGroupSDT")]
	DeviceDataSourceInstanceGroup,

	/// <summary>
	/// Device Data Source Instance
	/// </summary>
	[EnumMember(Value = "DeviceDataSourceInstanceSDT")]
	DeviceDataSourceInstance,

	/// <summary>
	/// Device Event Source
	/// </summary>
	[EnumMember(Value = "DeviceEventSourceSDT")]
	DeviceEventSource,

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
