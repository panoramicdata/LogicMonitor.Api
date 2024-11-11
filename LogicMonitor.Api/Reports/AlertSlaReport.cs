namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert SLA report
/// </summary>
[DataContract]
public class AlertSlaReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string ResourcesValue { get; set; } = string.Empty;

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string ResourcesValueType { get; set; } = string.Empty;

	/// <summary>
	/// The dataSource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; } = string.Empty;

	/// <summary>
	/// The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// The alertLevel
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public string AlertLevel { get; set; } = string.Empty;

	/// <summary>
	/// The alert Rule
	/// </summary>
	[DataMember(Name = "alertRule")]
	public string AlertRule { get; set; } = string.Empty;
}
