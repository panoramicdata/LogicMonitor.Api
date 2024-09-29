namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Device SLA widget metric
/// </summary>
[DataContract]
public class ResourceSlaWidgetMetric
{
	/// <summary>
	/// The groupName
	/// </summary>
	[DataMember(Name = "groupName")]
	public string ResourceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupName instead", true)]
	public string DeviceGroupName => ResourceGroupName;

	/// <summary>
	/// The Resource name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDisplayName instead", true)]
	public string DeviceName => ResourceDisplayName;

	/// <summary>
	/// The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string DataSourceInstances { get; set; } = string.Empty;

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
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; } = string.Empty;

	/// <summary>
	/// The bottom label
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; } = string.Empty;

	/// <summary>
	/// The exclusionSDTType
	/// </summary>
	[DataMember(Name = "exclusionSDTType")]
	public string ExclusionSdtType { get; set; } = string.Empty;
}
