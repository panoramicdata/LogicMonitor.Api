namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Service SDT
/// </summary>
[DataContract]
public class ServiceAlertSdt : AlertSdt
{
	/// <summary>
	/// The Collector description
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The Collector description
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
