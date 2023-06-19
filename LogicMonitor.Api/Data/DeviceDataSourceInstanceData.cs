namespace LogicMonitor.Api.Data;

/// <summary>
/// Time series graph data
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceData
{
	/// <summary>
	/// datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// datapoint values 2-D list
	/// </summary>
	[DataMember(Name = "values")]
	public List<List<float>> Values { get; set; } = new();

	/// <summary>
	/// timestamp list
	/// </summary>
	[DataMember(Name = "time")]
	public List<long> Time { get; set; } = new();

	/// <summary>
	/// the next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParams { get; set; } = string.Empty;
}
