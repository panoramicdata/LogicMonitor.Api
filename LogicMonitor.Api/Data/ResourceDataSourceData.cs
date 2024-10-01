namespace LogicMonitor.Api.Data;

/// <summary>
/// Resource datasource data
/// </summary>

[DataContract]
public class ResourceDataSourceData
{
	/// <summary>
	/// Instances
	/// </summary>
	[DataMember(Name = "instances")]
	public object Instances { get; set; } = new();

	/// <summary>
	/// Datapoints
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<string> DataPoints { get; set; } = [];

	/// <summary>
	/// Next page params
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParams { get; set; } = string.Empty;

	/// <summary>
	/// Datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;
}
