namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A device data source SDT
/// </summary>
[DataContract]
public class DeviceDataSourceAlertSdt : AlertSdt
{
	/// <summary>
	/// The DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource ID
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Device ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
