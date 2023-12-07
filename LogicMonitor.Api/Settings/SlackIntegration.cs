namespace LogicMonitor.Api.Settings;

/// <summary>
///     An Slack integration
/// </summary>
[DataContract]
public class SlackIntegration : Integration
{
	/// <summary>
	///     The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	///     The updateMethod
	/// </summary>
	[DataMember(Name = "updateMethod")]
	public string UpdateMethod { get; set; } = string.Empty;

	/// <summary>
	///     The update UserName
	/// </summary>
	[DataMember(Name = "updateUsername")]
	public string UpdateUserName { get; set; } = string.Empty;

	/// <summary>
	///     The clearMethod
	/// </summary>
	[DataMember(Name = "clearMethod")]
	public string ClearMethod { get; set; } = string.Empty;

	/// <summary>
	///     The ackMethod
	/// </summary>
	[DataMember(Name = "ackMethod")]
	public string AckMethod { get; set; } = string.Empty;

	/// <summary>
	///     The url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	///     The updateUrl
	/// </summary>
	[DataMember(Name = "updateUrl")]
	public string UpdateUrl { get; set; } = string.Empty;

	/// <summary>
	///     The clearUrl
	/// </summary>
	[DataMember(Name = "clearUrl")]
	public string ClearUrl { get; set; } = string.Empty;

	/// <summary>
	///     The ackUrl
	/// </summary>
	[DataMember(Name = "ackUrl")]
	public string AckUrl { get; set; } = string.Empty;

	/// <summary>
	///     The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	///     The updateUername
	/// </summary>
	[DataMember(Name = "updateUername")]
	public string UpdateUername { get; set; } = string.Empty;

	/// <summary>
	///     The clearUsername
	/// </summary>
	[DataMember(Name = "clearUsername")]
	public string ClearUsername { get; set; } = string.Empty;

	/// <summary>
	///     The ackUsername
	/// </summary>
	[DataMember(Name = "ackUsername")]
	public string AckUsername { get; set; } = string.Empty;

	/// <summary>
	///     The password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	///     The updatePassword
	/// </summary>
	[DataMember(Name = "updatePassword")]
	public string UpdatePassword { get; set; } = string.Empty;

	/// <summary>
	///     The clearPassword
	/// </summary>
	[DataMember(Name = "clearPassword")]
	public string ClearPassword { get; set; } = string.Empty;

	/// <summary>
	///     The ackPassword
	/// </summary>
	[DataMember(Name = "ackPassword")]
	public string AckPassword { get; set; } = string.Empty;

	/// <summary>
	///     The payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string Payload { get; set; } = string.Empty;

	/// <summary>
	///     The clearPayload
	/// </summary>
	[DataMember(Name = "clearPayload")]
	public string ClearPayload { get; set; } = string.Empty;

	/// <summary>
	///     The updatePayload
	/// </summary>
	[DataMember(Name = "updatePayload")]
	public string UpdatePayload { get; set; } = string.Empty;

	/// <summary>
	///     The ackPayload
	/// </summary>
	[DataMember(Name = "ackPayload")]
	public string AckPayload { get; set; } = string.Empty;

	/// <summary>
	///     The payloadFormat
	/// </summary>
	[DataMember(Name = "payloadFormat")]
	public string PayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The updatePayloadFormat
	/// </summary>
	[DataMember(Name = "updatePayloadFormat")]
	public string UpdatePayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The clearPayloadFormat
	/// </summary>
	[DataMember(Name = "clearPayloadFormat")]
	public string ClearPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The ackPayloadFormat
	/// </summary>
	[DataMember(Name = "ackPayloadFormat")]
	public string AckPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The headers
	/// </summary>
	[DataMember(Name = "headers")]
	public List<object> Headers { get; set; } = [];

	/// <summary>
	///     The updateHeaders
	/// </summary>
	[DataMember(Name = "updateHeaders")]
	public List<object> UpdateHeaders { get; set; } = [];

	/// <summary>
	///     The clearHeaders
	/// </summary>
	[DataMember(Name = "clearHeaders")]
	public List<object> ClearHeaders { get; set; } = [];

	/// <summary>
	///     The ackHeaders
	/// </summary>
	[DataMember(Name = "ackHeaders")]
	public List<object> AckHeaders { get; set; } = [];

	/// <summary>
	///     The parseMethod
	/// </summary>
	[DataMember(Name = "parseMethod")]
	public string ParseMethod { get; set; } = string.Empty;

	/// <summary>
	///     The parseExpression
	/// </summary>
	[DataMember(Name = "parseExpression")]
	public List<object> ParseExpression { get; set; } = [];

	/// <summary>
	///     The enabledStatus
	/// </summary>
	[DataMember(Name = "enabledStatus")]
	public List<string> EnabledStatus { get; set; } = [];

	/// <summary>
	///     The incomingWebhookUrl
	/// </summary>
	[DataMember(Name = "incomingWebhookUrl")]
	public string IncomingWebhookUrl { get; set; } = string.Empty;
}
