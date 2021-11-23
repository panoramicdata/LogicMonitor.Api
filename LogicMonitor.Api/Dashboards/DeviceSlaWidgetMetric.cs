using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Device SLA widget metric
/// </summary>
[DataContract]
public class DeviceSlaWidgetMetric
{
	/// <summary>
	/// The groupName
	/// </summary>
	[DataMember(Name = "groupName")]
	public string DeviceGroupName { get; set; }

	/// <summary>
	/// The deviceName
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string DeviceName { get; set; }

	/// <summary>
	/// The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; }

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string DataSourceInstances { get; set; }

	/// <summary>
	/// The metric
	/// </summary>
	[DataMember(Name = "metric")]
	public string Metric { get; set; }

	/// <summary>
	/// The threshold
	/// </summary>
	[DataMember(Name = "threshold")]
	public string Threshold { get; set; }

	/// <summary>
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; }

	/// <summary>
	/// The bottom label
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; }

	/// <summary>
	/// The exclusionSDTType
	/// </summary>
	[DataMember(Name = "exclusionSDTType")]
	public string ExclusionSdtType { get; set; }
}
