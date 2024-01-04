namespace LogicMonitor.Api.Settings;

/// <summary>
/// A LogicMonitor alert rule creation DTO
/// </summary>
[DataContract]
public class AlertRuleCreationDto : CreationDto<AlertRule>
{
	/// <summary>
	///    The LogicMonitor Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The priority
	/// </summary>
	[DataMember(Name = "priority")]
	public int Priority { get; set; }

	/// <summary>
	/// The level (as a string)
	/// </summary>
	[DataMember(Name = "levelStr")]
	public string LevelString { get; set; } = string.Empty;

	/// <summary>
	/// The device filter
	/// </summary>
	[DataMember(Name = "devices")]
	public List<string> Devices { get; set; } = [];

	/// <summary>
	/// The device group filter
	/// </summary>
	[DataMember(Name = "deviceGroups")]
	public List<string> DeviceGroups { get; set; } = [];

	/// <summary>
	/// The affected DataSource name filter
	/// </summary>
	[DataMember(Name = "datasource")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The data source instance name filter
	/// </summary>
	[DataMember(Name = "instance")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The datapoint filter
	/// </summary>
	[DataMember(Name = "datapoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// The escalation chain interval in minutes
	/// </summary>
	[DataMember(Name = "escalationInterval")]
	public int EscalationChainIntervalMinutes { get; set; }

	/// <summary>
	/// The Escalating Chain Id
	/// </summary>
	[DataMember(Name = "escalatingChainId")]
	public int EscalationChainId { get; set; }

	/// <summary>
	/// The Escalation Chain
	/// </summary>
	[DataMember(Name = "escalatingChain")]
	public EscalationChain EscalationChain { get; set; } = new();

	/// <summary>
	/// The resource property filters list
	/// </summary>
	[DataMember(Name = "resourceProperties")]
	public List<DeviceProperty> ResourceProperties { get; set; } = [];

	/// <summary>
	///  send anomaly suppressed alert
	/// </summary>
	[DataMember(Name = "sendAnomalySuppressedAlert")]
	public bool SendAnomalySuppressedAlert { get; set; }

	/// <summary>
	/// Whether to suppress Alert clears
	/// </summary>
	[DataMember(Name = "suppressAlertClear")]
	public bool SuppressAlertClear { get; set; }

	/// <summary>
	/// Whether to suppress Alert ack sdts
	/// </summary>
	[DataMember(Name = "suppressAlertAckSdt")]
	public bool SuppressAlertAckSdt { get; set; }
}