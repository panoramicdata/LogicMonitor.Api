namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DataSource Graph DataPoint
/// </summary>
[DataContract]
public class GraphLine
{
	/// <summary>
	/// the graph line color name
	/// </summary>
	[DataMember(Name = "colorName")]
	public string ColorName { get; set; } = string.Empty;

	/// <summary>
	/// the graph line data point id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// the graph line data point name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// if the graph line\u0027s data point is a virtual data point
	/// </summary>
	[DataMember(Name = "isVirtualDataPoint")]
	public bool IsVirtualDataPoint { get; set; }

	/// <summary>
	/// the graph line legend
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; } = string.Empty;

	/// <summary>
	/// the graph line type, 1\u003dline|2\u003darea|3\u003dstack|4\u003dcolumn
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// ToString override
	/// </summary>
	public override string ToString() => $"{Legend} ({ColorName})";
}
