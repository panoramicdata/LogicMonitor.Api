namespace LogicMonitor.Api.Reports;

/// <summary>
/// A ServiceLevelAgreementReportMetric
/// </summary>
[DataContract]
public class SlaReportMetric
{
	/// <summary>
	/// The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string DeviceName { get; set; } = string.Empty;

	/// <summary>
	/// The data source id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The datasource
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string Instances { get; set; } = string.Empty;

	/// <summary>
	/// The metric
	/// </summary>
	[DataMember(Name = "metric")]
	public string Metric { get; set; } = string.Empty;

	/// <summary>
	/// The threshold
	/// </summary>
	[DataMember(Name = "threshold")]
	public string Threshold { get; set; } = string.Empty;

	/// <summary>
	/// The exclusion SDT type
	/// </summary>
	[DataMember(Name = "exclusionSDTType")]
	public string ExclusionSdtType { get; set; } = string.Empty;
}
