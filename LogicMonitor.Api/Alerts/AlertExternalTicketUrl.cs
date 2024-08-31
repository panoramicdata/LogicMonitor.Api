namespace LogicMonitor.Api.Alerts;

/// <summary>
/// AlertExternalTicketUrl
/// </summary>
[DataContract]
public class AlertExternalTicketUrl
{
	/// <summary>
	/// The alert id
	/// </summary>
	[DataMember(Name = "servicenowIncidentLinks")]
	public AlertExternalTicketUrlServiceNowIncidentLinks? ServiceNowIncidentLinks { get; set; }
}