namespace LogicMonitor.Api.Settings;

/// <summary>
/// IntegrationAuditLog
/// </summary>
[DataContract]

public class IntegrationAuditLog : IdentifiedItem
{
	/// <summary>
	/// HTTP request headers used in alert delivery
	/// </summary>
	[DataMember(Name = "headers", IsRequired = false)]
	public string? Headers { get; set; }

	/// <summary>
	/// The type of the alert
	/// </summary>
	[DataMember(Name = "alertType", IsRequired = false)]
	public int AlertType { get; set; }

	/// <summary>
	/// The name of integration
	/// </summary>
	[DataMember(Name = "integrationName", IsRequired = false)]
	public string? IntegrationName { get; set; }

	/// <summary>
	/// The number of times delivery was retried
	/// </summary>
	[DataMember(Name = "numRetries", IsRequired = false)]
	public int NumRetries { get; set; }

	/// <summary>
	/// The Integration Alert Status used for delivery
	/// </summary>
	[DataMember(Name = "integrationAlertStatus", IsRequired = false)]
	public string? IntegrationAlertStatus { get; set; }

	/// <summary>
	/// Error message (if any) from ADC
	/// </summary>
	[DataMember(Name = "errorMessage", IsRequired = false)]
	public string? ErrorMessage { get; set; }

	/// <summary>
	/// The parsed External Ticket ID from alert delivery
	/// </summary>
	[DataMember(Name = "externalTicketId", IsRequired = false)]
	public string? ExternalTicketId { get; set; }

	/// <summary>
	/// The outbound payload format
	/// </summary>
	[DataMember(Name = "payloadFormat", IsRequired = false)]
	public string? PayloadFormat { get; set; }

	/// <summary>
	/// The URL where the alert was delivered to
	/// </summary>
	[DataMember(Name = "url", IsRequired = false)]
	public string? Url { get; set; }

	/// <summary>
	/// When the delivery result was saved in LMES
	/// </summary>
	[DataMember(Name = "happenedOnMs", IsRequired = false)]
	public long HappenedOnMs { get; set; }

	/// <summary>
	/// The type of integration
	/// </summary>
	[DataMember(Name = "integrationType", IsRequired = false)]
	public string? IntegrationType { get; set; }

	/// <summary>
	/// The id of the alert instance
	/// </summary>
	[DataMember(Name = "alertInstanceId", IsRequired = false)]
	public string? AlertInstanceId { get; set; }

	/// <summary>
	/// The HTTP Request payload
	/// </summary>
	[DataMember(Name = "payload", IsRequired = false)]
	public string? Payload { get; set; }

	/// <summary>
	/// The HTTP Response Code received from 3rd party API
	/// </summary>
	[DataMember(Name = "httpResponseCode", IsRequired = false)]
	public int HttpResponseCode { get; set; }

	/// <summary>
	/// The id of the alert
	/// </summary>
	[DataMember(Name = "alertId", IsRequired = false)]
	public string? AlertId { get; set; }

	/// <summary>
	/// The HTTP Response Body received after alert delivery
	/// </summary>
	[DataMember(Name = "httpResponse", IsRequired = false)]
	public string? HttpResponse { get; set; }
}
