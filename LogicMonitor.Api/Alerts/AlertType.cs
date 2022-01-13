namespace LogicMonitor.Api.Alerts;

/// <summary>
///    The REST Alert Type
/// </summary>
[DataContract]
public enum AlertType
{
	/// <summary>
	///    Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Batch job alert
	/// </summary>
	[EnumMember(Value = "dataSourceAlert")]
	DataSource = 1,

	/// <summary>
	///    EventSource alert
	/// </summary>
	[EnumMember(Value = "eventAlert")]
	EventSource = 2,

	/// <summary>
	///    Website alert
	/// </summary>
	[EnumMember(Value = "websiteAlert")]
	Website = 3,

	/// <summary>
	///    Host cluster alert
	/// </summary>
	[EnumMember(Value = "hostClusterAlert")]
	DeviceCluster = 4,

	/// <summary>
	///    Batch job alert
	/// </summary>
	[EnumMember(Value = "batchJobAlert")]
	BatchJob = 5,

	/// <summary>
	///    Collector down alert
	/// </summary>
	[EnumMember(Value = "agentDownAlert")]
	CollectorDown = 6,

	/// <summary>
	///    Collector failover alert
	/// </summary>
	[EnumMember(Value = "agentFailoverAlert")]
	CollectorFailover = 7,

	/// <summary>
	///    Collector fail back alert
	/// </summary>
	[EnumMember(Value = "agentFailBackAlert")]
	CollectorFailBack = 8,

	/// <summary>
	///    Alert throttled alert
	/// </summary>
	[EnumMember(Value = "alertThrottledAlert")]
	AlertThrottled = 9,

	/// <summary>
	///    Log alert
	/// </summary>
	[EnumMember(Value = "logAlert")]
	Log = 0
}
