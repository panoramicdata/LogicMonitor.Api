namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The DataSource Resource
/// </summary>
[DataContract]
public class DataSourceResource
{
	/// <summary>
	/// The deviceDataSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The Resource DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;
}
