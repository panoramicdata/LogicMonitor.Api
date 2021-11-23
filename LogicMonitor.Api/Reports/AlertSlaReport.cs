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
	public string DevicesValue { get; set; }

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string DevicesValueType { get; set; }

	/// <summary>
	/// The dataSource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; }

	/// <summary>
	/// The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; }

	/// <summary>
	/// The alertLevel
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public string AlertLevel { get; set; }

	/// <summary>
	/// The alert Rule
	/// </summary>
	[DataMember(Name = "alertRule")]
	public string AlertRule { get; set; }
}
