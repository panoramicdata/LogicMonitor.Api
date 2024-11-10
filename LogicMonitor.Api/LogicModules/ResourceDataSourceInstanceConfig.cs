namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    A ResourceDataSourceInstance instance config
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceConfig : StringIdentifiedItem
{
	/// <summary>
	/// Alerts associated to this configuration file
	/// </summary>
	[DataMember(Name = "alerts")]
	public List<ResourceDataSourceInstanceConfigAlert> Alerts { get; set; } = [];

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
	/// DataSource id
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
	public List<ResourceDataSourceInstanceConfigDiff> DeltaConfig { get; set; } = [];

	/// <summary>
	/// Resource datasource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? ResourceId { get; set; }

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
