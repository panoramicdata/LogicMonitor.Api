namespace LogicMonitor.Api.Websites;

/// <summary>
/// WebsiteCheckpointRawData
/// </summary>

[DataContract]
public class WebsiteCheckpointRawData
{
	/// <summary>
	/// datapoint values 2-D list
	/// </summary>
	[DataMember(Name = "values")]
	public List<object>? Values { get; set; }

	/// <summary>
	/// timestamp list
	/// </summary>
	[DataMember(Name = "time")]
	public List<long>? Time { get; set; }

	/// <summary>
	/// the next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string? NextPageParams { get; set; }
}
