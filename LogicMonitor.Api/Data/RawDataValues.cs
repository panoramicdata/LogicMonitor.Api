namespace LogicMonitor.Api.Data;

/// <summary>
/// Raw data values
/// </summary>

[DataContract]
public class RawDataValues
{
	/// <summary>
	/// datapoint values 2-D list
	/// </summary>
	[DataMember(Name = "values")]
	public List<List<object>> Values { get; set; } = new();

	/// <summary>
	/// timestamp list
	/// </summary>
	[DataMember(Name = "time")]
	public List<int> Time { get; set; } = new();

	/// <summary>
	/// the next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParams { get; set; } = string.Empty;
}
