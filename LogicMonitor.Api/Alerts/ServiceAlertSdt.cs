namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Service SDT
/// </summary>
[DataContract]
public class ServiceAlertSdt : AlertSdt
{
	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
