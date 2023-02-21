namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceInstanceConfigAlert
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceConfigAlert : IdentifiedItem
{
	/// <summary>
	/// Alert level, 0 - alert is cleared, 2 - warn alert, 3 - error alert, 4 - critical alert
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public int AlertLevel { get; set; }

	/// <summary>
	/// Alert internal id
	/// </summary>
	[DataMember(Name = "alertId")]
	public string AlertId { get; set; }

	/// <summary>
	/// Summary of this config source alert
	/// </summary>
	[DataMember(Name = "alertSummary")]
	public string AlertSummary { get; set; }

	/// <summary>
	/// Timestamp of alert start or clear
	/// </summary>
	[DataMember(Name = "timestamp")]
	public int Timestamp { get; set; }
}
