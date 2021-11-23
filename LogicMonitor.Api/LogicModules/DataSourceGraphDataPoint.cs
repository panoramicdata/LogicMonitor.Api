namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DataSource Graph DataPoint
/// </summary>
[DataContract]
public class DataSourceGraphDataPoint : NamedItem
{
	/// <summary>
	/// The DataPoint Id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The DataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	/// The DataPoint name
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	/// Aggregation
	/// </summary>
	[DataMember(Name = "cf")]
	public string Aggregation { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	public override string ToString() => $"{Name} (Id)";
}
