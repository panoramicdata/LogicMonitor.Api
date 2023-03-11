namespace LogicMonitor.Api.Settings;

/// <summary>
///     An HTTP integration
/// </summary>
[DataContract]
public class HttpIntegration : Integration
{
	/// <summary>
	///     The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	///     The update method
	/// </summary>
	[DataMember(Name = "updateMethod")]
	public string UpdateMethod { get; set; } = string.Empty;

	/// <summary>
	///     The clear method
	/// </summary>
	[DataMember(Name = "clearMethod")]
	public string ClearMethod { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement method
	/// </summary>
	[DataMember(Name = "ackMethod")]
	public string AckMethod { get; set; } = string.Empty;

	/// <summary>
	///     The Url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	///     The update Url
	/// </summary>
	[DataMember(Name = "updateUrl")]
	public string UpdateUrl { get; set; } = string.Empty;

	/// <summary>
	///     The clear Url
	/// </summary>
	[DataMember(Name = "clearUrl")]
	public string ClearUrl { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Url
	/// </summary>
	[DataMember(Name = "ackUrl")]
	public string AckUrl { get; set; } = string.Empty;

	/// <summary>
	///     The Username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	///     The update Username
	/// </summary>
	[DataMember(Name = "updateUsername")]
	public string UpdateUsername { get; set; } = string.Empty;

	/// <summary>
	///     The clear Username
	/// </summary>
	[DataMember(Name = "clearUsername")]
	public string ClearUsername { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Username
	/// </summary>
	[DataMember(Name = "ackUsername")]
	public string AckUsername { get; set; } = string.Empty;

	/// <summary>
	///     The Password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	///     The update Password
	/// </summary>
	[DataMember(Name = "updatePassword")]
	public string UpdatePassword { get; set; } = string.Empty;

	/// <summary>
	///     The clear Password
	/// </summary>
	[DataMember(Name = "clearPassword")]
	public string ClearPassword { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Password
	/// </summary>
	[DataMember(Name = "ackPassword")]
	public string AckPassword { get; set; } = string.Empty;

	/// <summary>
	///     The Payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string Payload { get; set; } = string.Empty;

	/// <summary>
	///     The update Payload
	/// </summary>
	[DataMember(Name = "updatePayload")]
	public string UpdatePayload { get; set; } = string.Empty;

	/// <summary>
	///     The clear Payload
	/// </summary>
	[DataMember(Name = "clearPayload")]
	public string ClearPayload { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Payload
	/// </summary>
	[DataMember(Name = "ackPayload")]
	public string AckPayload { get; set; } = string.Empty;

	/// <summary>
	///     The PayloadFormat
	/// </summary>
	[DataMember(Name = "payloadFormat")]
	public string PayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The update PayloadFormat
	/// </summary>
	[DataMember(Name = "updatePayloadFormat")]
	public string UpdatePayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The clear PayloadFormat
	/// </summary>
	[DataMember(Name = "clearPayloadFormat")]
	public string ClearPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement PayloadFormat
	/// </summary>
	[DataMember(Name = "ackPayloadFormat")]
	public string AckPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public List<object> Headers { get; set; } = new();

	/// <summary>
	///     The update Headers
	/// </summary>
	[DataMember(Name = "updateHeaders")]
	public List<object> UpdateHeaders { get; set; } = new();

	/// <summary>
	///     The clear Headers
	/// </summary>
	[DataMember(Name = "clearHeaders")]
	public List<object> ClearHeaders { get; set; } = new();

	/// <summary>
	///     The acknowledgement Headers
	/// </summary>
	[DataMember(Name = "ackHeaders")]
	public List<object> AckHeaders { get; set; } = new();

	/// <summary>
	///     The parse method
	/// </summary>
	[DataMember(Name = "parseMethod")]
	public string ParseMethod { get; set; } = string.Empty;

	/// <summary>
	///     The parse expression
	/// </summary>
	[DataMember(Name = "parseExpression")]
	public string ParseExpression { get; set; } = string.Empty;

	/// <summary>
	///     The parse method
	/// </summary>
	[DataMember(Name = "enabledStatus")]
	public List<string> EnabledStatus { get; set; } = new();
}
