namespace LogicMonitor.Api.Data;

/// <summary>
/// An instance FetchData response
/// </summary>
[DataContract]
public class InstanceFetchDataResponse
{
	/// <summary>
	/// DeviceDataSourceInstanceId
	/// </summary>
	[DataMember(Name = "instanceId")]
	public string DeviceDataSourceInstanceId { get; set; } = string.Empty;

	/// <summary>
	/// Error message
	/// </summary>
	[DataMember(Name = "errMsg")]
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	/// DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// Data Points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<string> DataPoints { get; set; } = new();

	/// <summary>
	/// Data Values
	/// </summary>
	[DataMember(Name = "values")]
	public List<List<object>> DataValues { get; set; } = new();

	/// <summary>
	/// Timestamps
	/// </summary>
	[DataMember(Name = "time")]
	public List<long> Timestamps { get; set; } = new();

	/// <summary>
	/// Next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParameters { get; set; } = string.Empty;
}
