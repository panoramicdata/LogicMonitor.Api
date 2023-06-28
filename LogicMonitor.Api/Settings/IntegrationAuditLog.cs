namespace LogicMonitor.Api.Settings;

/// <summary>
/// IntegrationAuditLog
/// </summary>
[DataContract]

public class IntegrationAuditLog : IdentifiedItemBase<string>
{
	/// <summary>
	/// HTTP request headers used in alert delivery
	/// </summary>
	[DataMember(Name = "headers")]
	public string? Headers { get; set; }

	/// <summary>
	/// The type of the alert
	/// </summary>
	[DataMember(Name = "alertType")]
	public int AlertType { get; set; }

	/// <summary>
	/// The name of integration
	/// </summary>
	[DataMember(Name = "integrationName")]
	public string? IntegrationName { get; set; }

	/// <summary>
	/// The number of times delivery was retried
	/// </summary>
	[DataMember(Name = "numRetries")]
	public int NumRetries { get; set; }

	/// <summary>
	/// The Integration Alert Status used for delivery
	/// </summary>
	[DataMember(Name = "integrationAlertStatus")]
	public string? IntegrationAlertStatus { get; set; }

	/// <summary>
	/// Error message (if any) from ADC
	/// </summary>
	[DataMember(Name = "errorMessage")]
	public string? ErrorMessage { get; set; }

	/// <summary>
	/// The parsed External Ticket ID from alert delivery
	/// </summary>
	[DataMember(Name = "externalTicketId")]
	public string? ExternalTicketId { get; set; }

	/// <summary>
	/// The outbound payload format
	/// </summary>
	[DataMember(Name = "payloadFormat")]
	public string? PayloadFormat { get; set; }

	/// <summary>
	/// The URL where the alert was delivered to
	/// </summary>
	[DataMember(Name = "url")]
	public string? Url { get; set; }

	/// <summary>
	/// When the delivery result was saved in LMES
	/// </summary>
	[DataMember(Name = "happenedOnMs")]
	public long HappenedOnMs { get; set; }

	/// <summary>
	/// The type of integration
	/// </summary>
	[DataMember(Name = "integrationType")]
	public string? IntegrationType { get; set; }

	/// <summary>
	/// The id of the alert instance
	/// </summary>
	[DataMember(Name = "alertInstanceId")]
	public string? AlertInstanceId { get; set; }

	/// <summary>
	/// The HTTP Request payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string? Payload { get; set; }

	/// <summary>
	/// The HTTP Response Code received from 3rd party API
	/// </summary>
	[DataMember(Name = "httpResponseCode")]
	public int HttpResponseCode { get; set; }

	/// <summary>
	/// The id of the alert
	/// </summary>
	[DataMember(Name = "alertId")]
	public string? AlertId { get; set; }

	/// <summary>
	/// The HTTP Response Body received after alert delivery
	/// </summary>
	[DataMember(Name = "httpResponse")]
	public string? HttpResponse { get; set; }
}
