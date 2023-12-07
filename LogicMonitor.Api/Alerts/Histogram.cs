namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A histogram containing the historical information requested
/// </summary>
[DataContract]
public class Histogram
{
	/// <summary>
	/// The legends for the data series that was returned
	/// </summary>
	[DataMember(Name = "datapoint")]
	public List<string> Datapoint { get; set; } = [];

	/// <summary>
	/// The timestamps of the series that was returned
	/// </summary>
	[DataMember(Name = "timestamps")]
	public List<int> Timestamps { get; set; } = [];

	/// <summary>
	/// The values of the series that was returned
	/// </summary>
	[DataMember(Name = "values")]
	public List<List<int>> Values { get; set; } = [];
}
