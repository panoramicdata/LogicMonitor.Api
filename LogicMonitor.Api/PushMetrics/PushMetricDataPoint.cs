namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric DataPoint
/// </summary>
[DataContract]
public class PushMetricDataPoint
{
	/// <summary>
	/// Datapoint name.
	/// If no existing datapoint matches for specified DataSource, a new datapoint is created with this name.
	/// Required
	/// * 128-character limit
	/// * Characters from A-Z, a-z, and 0-9 only allowed
	/// * Should not contain reserved keywords such as COS, SIN, etc.
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string Name { get; set; }

	/// <summary>
	/// Datapoint description.
	/// Only considered when creating a new datapoint.
	/// Optional. Defaults to {dataPointName}.
	/// * 1024-character limit
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string Description { get; set; }

	/// <summary>
	/// Metric type as a number in string format.
	/// Only considered when creating a new datapoint.
	/// Optional. Defaults to "gauge".
	/// * Only values of  "counter", "derive", or "gauge" accepted
	/// * Case insensitive
	/// </summary>
	[DataMember(Name = "dataPointDataType")]
	public PushMetricDataPointDataType DataType { get; set; }

	/// <summary>
	/// The DataSource instances
	/// The aggregation method, if any, that should be used if data is pushed in sub-minute intervals.
	/// Only considered when creating a new datapoint.
	/// See the About the Push Metrics REST API section of this guide for more information on datapoint value aggregation intervals.
	/// Optional. Defaults to "none".
	/// * Only values of  "none", "avg", or "sum" accepted
	/// * Case insensitive
	/// </summary>
	[DataMember(Name = "dataPointAggregationType")]
	public PushMetricAggregationType? AggregationType { get; set; }

	/// <summary>
	/// An array of datapoint values.
	/// You may wish to use one of the extension methods:
	/// - (Dictionary&lt;DateTimeOffset, double&gt;).ToLogicMonitorDictionary()
	/// - (Dictionary&lt;DateTimeOffset, float&gt;).ToLogicMonitorDictionary()
	/// - (Dictionary&lt;DateTimeOffset, int&gt;).ToLogicMonitorDictionary()
	/// Required
	/// * Takes input as key-value pairs in the form of epoch time and datapoint value.Example:  "1584902069" : "10"
	/// * Only long type values accepted in keys
	/// * Only digits accepted in values
	/// </summary>
	[DataMember(Name = "values")]
	public Dictionary<string, string> Values { get; set; }
}
