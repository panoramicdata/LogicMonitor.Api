namespace LogicMonitor.Api.Resources;

/// <summary>
/// ResourceDataSourceInstanceConfigAlert
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceConfigAlert : StringIdentifiedItem
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
	public string AlertId { get; set; } = string.Empty;

	/// <summary>
	/// Summary of this config source alert
	/// </summary>
	[DataMember(Name = "alertSummary")]
	public string AlertSummary { get; set; } = string.Empty;

	/// <summary>
	/// Timestamp of alert start or clear
	/// </summary>
	[DataMember(Name = "timestamp")]
	public int Timestamp { get; set; }
}
