namespace LogicMonitor.Api.Reports;

/// <summary>
/// A AlertTrendReportMetric
/// </summary>
[DataContract]
public class AlertTrendsReportMetric
{
	/// <summary>
	/// The item type
	/// </summary>
	[DataMember(Name = "itemType")]
	public string ItemType { get; set; } = string.Empty;

	/// <summary>
	/// The item value
	/// </summary>
	[DataMember(Name = "itemVal")]
	public string ItemValue { get; set; } = string.Empty;
}
