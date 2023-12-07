namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Big Number Info
/// </summary>
[DataContract]
public class BigNumberInfo
{
	/// <summary>
	/// The big number items
	/// </summary>
	[DataMember(Name = "bigNumberItems")]
	public List<BigNumberItem> BigNumberItems { get; set; } = [];

	/// <summary>
	/// The big number items
	/// </summary>
	[DataMember(Name = "counters")]
	public List<BigNumberCounter> Counter { get; set; } = [];

	/// <summary>
	/// The datapoints
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<BigNumberDataPoint> DataPoints { get; set; } = [];

	/// <summary>
	/// The virtual datapoints
	/// </summary>
	[DataMember(Name = "virtualDataPoints")]
	public List<VirtualDataPoint> VirtualDataPoints { get; set; } = [];
}
