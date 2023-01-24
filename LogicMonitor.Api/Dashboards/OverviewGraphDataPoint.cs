using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// OverviewGraphDataPoint
/// </summary>

[DataContract]
public class OverviewGraphDataPoint
{
	/// <summary>
	/// the graph line data point aggregate method, average|min|max|sum
	/// </summary>
	[DataMember(Name = "aggregateMethod")]
	public string? AggregateMethod { get; set; }

	/// <summary>
	/// the graph line data point name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	/// the graph line data point id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// the graph line data point consolidate function, 1\u003davg|2\u003dmax|3\u003dmin
	/// </summary>
	[DataMember(Name = "consolidateFunc")]
	public int ConsolidateFunc { get; set; }

	/// <summary>
	/// dataSourceDataPointId
	/// </summary>
	[DataMember(Name = "dataSourceDataPointId")]
	public int DataSourceDataPointId { get; set; }
}
