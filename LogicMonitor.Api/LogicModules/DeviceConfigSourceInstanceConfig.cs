namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    A DeviceDataSourceInstance
/// </summary>
[DataContract]
public class DeviceConfigSourceInstanceConfig : StringIdentifiedItem
{
	/// <summary>
	///    Alert Disable Status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///    Alert Disable Status
	/// </summary>
	[DataMember(Name = "alerts")]
	public List<object> Alerts { get; set; } // TODO - complete

	/// <summary>
	///    Config
	/// </summary>
	[DataMember(Name = "config")]
	public string Config { get; set; }

	/// <summary>
	///    The change status
	/// </summary>
	[DataMember(Name = "changeStatus")]
	public string ChangeStatus { get; set; }

	/// <summary>
	///    ConfigStatus
	/// </summary>
	[DataMember(Name = "configStatus")]
	public int ConfigStatus { get; set; }

	/// <summary>
	///    Config error message
	/// </summary>
	[DataMember(Name = "configErrMsg")]
	public string ConfigErrorMessage { get; set; }

	/// <summary>
	///    The ConfigSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	///    The ConfigSource Name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; }

	/// <summary>
	///    The delta config
	/// </summary>
	[DataMember(Name = "deltaConfig")]
	public List<ConfigSourceDeltaConfig> DeltaConfig { get; set; }

	/// <summary>
	///    The DeviceConfigSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	///    The device DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	///    The Device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? DeviceId { get; set; }

	/// <summary>
	///    The InstanceId
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int InstanceId { get; set; }

	/// <summary>
	///    The instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; }

	/// <summary>
	///    Poll timestamp
	/// </summary>
	[DataMember(Name = "pollTimestamp")]
	public long? PollTimestampUtc { get; set; }

	/// <summary>
	///    The pollTimestamp DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime? PollUtc => PollTimestampUtc?.ToDateTimeUtc();

	/// <summary>
	///    The version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; }
}
