namespace LogicMonitor.Api.Alerts;

/// <summary>
/// The history associated with an alert
/// </summary>
[DataContract]
public class AlertHistory
{
	/// <summary>
	/// The number of alerts that occurred in the requested period
	/// </summary>
	[DataMember(Name = "alertCount")]
	public int AlertCount { get; set; }

	/// <summary>
	/// The histogram detailing the distribution of the alerts in the period
	/// </summary>
	[DataMember(Name = "histogram")]
	public Histogram Histogram { get; set; } 

	/// <summary>
	/// The type of the alert
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } 
}
