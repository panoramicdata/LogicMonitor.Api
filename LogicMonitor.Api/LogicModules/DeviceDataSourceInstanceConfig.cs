namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    A DeviceDataSourceInstance
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceConfig : StringIdentifiedItem
{
	/// <summary>
	/// Alerts associated to this configuration file
	/// </summary>
	[DataMember(Name = "alerts")]
	public List<DeviceDataSourceInstanceConfigAlert> Alerts { get; set; } = [];

	/// <summary>
	/// Configuration file content
	/// </summary>
	[DataMember(Name = "config")]
	public string Config { get; set; } = string.Empty;

	/// <summary>
	/// Configuration file change status, if the first configuration then it is Added, else Changed, values can be : Add|Change
	/// </summary>
	[DataMember(Name = "changeStatus")]
	public string ChangeStatus { get; set; } = string.Empty;

	/// <summary>
	/// Configuration file collect status
	/// </summary>
	[DataMember(Name = "configStatus")]
	public int ConfigStatus { get; set; }

	/// <summary>
	/// Configuration file collect error message
	/// </summary>
	[DataMember(Name = "configErrMsg")]
	public string ConfigErrorMessage { get; set; } = string.Empty;

	/// <summary>
	/// configsource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// Configuration file diff
	/// </summary>
	[DataMember(Name = "deltaConfig")]
	public List<DeviceDataSourceInstanceConfigDiff> DeltaConfig { get; set; } = [];

	/// <summary>
	/// device datasource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	/// device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? DeviceId { get; set; }

	/// <summary>
	/// advanceDiffChecker
	/// </summary>
	[DataMember(Name = "excludeLines")]
	public List<int> ExcludeLines { get; set; } = Enumerable.Empty<int>().ToList();

	/// <summary>
	/// device datasource instance id
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int InstanceId { get; set; }

	/// <summary>
	/// device datasource instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	/// datasource poll timestamp in milliseconds
	/// </summary>
	[DataMember(Name = "pollTimestamp")]
	public long? PollTimestampUtc { get; set; }

	/// <summary>
	///    The pollTimestamp DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime? PollUtc => PollTimestampUtc?.ToDateTimeUtc();

	/// <summary>
	/// config version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;
}
