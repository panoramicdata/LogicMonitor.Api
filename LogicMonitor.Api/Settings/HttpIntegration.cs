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
	public string Method { get; set; }

	/// <summary>
	///     The update method
	/// </summary>
	[DataMember(Name = "updateMethod")]
	public string UpdateMethod { get; set; }

	/// <summary>
	///     The clear method
	/// </summary>
	[DataMember(Name = "clearMethod")]
	public string ClearMethod { get; set; }

	/// <summary>
	///     The acknowledgement method
	/// </summary>
	[DataMember(Name = "ackMethod")]
	public string AckMethod { get; set; }

	/// <summary>
	///     The Url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }

	/// <summary>
	///     The update Url
	/// </summary>
	[DataMember(Name = "updateUrl")]
	public string UpdateUrl { get; set; }

	/// <summary>
	///     The clear Url
	/// </summary>
	[DataMember(Name = "clearUrl")]
	public string ClearUrl { get; set; }

	/// <summary>
	///     The acknowledgement Url
	/// </summary>
	[DataMember(Name = "ackUrl")]
	public string AckUrl { get; set; }

	/// <summary>
	///     The Username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; }

	/// <summary>
	///     The update Username
	/// </summary>
	[DataMember(Name = "updateUsername")]
	public string UpdateUsername { get; set; }

	/// <summary>
	///     The clear Username
	/// </summary>
	[DataMember(Name = "clearUsername")]
	public string ClearUsername { get; set; }

	/// <summary>
	///     The acknowledgement Username
	/// </summary>
	[DataMember(Name = "ackUsername")]
	public string AckUsername { get; set; }

	/// <summary>
	///     The Password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; }

	/// <summary>
	///     The update Password
	/// </summary>
	[DataMember(Name = "updatePassword")]
	public string UpdatePassword { get; set; }

	/// <summary>
	///     The clear Password
	/// </summary>
	[DataMember(Name = "clearPassword")]
	public string ClearPassword { get; set; }

	/// <summary>
	///     The acknowledgement Password
	/// </summary>
	[DataMember(Name = "ackPassword")]
	public string AckPassword { get; set; }

	/// <summary>
	///     The Payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string Payload { get; set; }

	/// <summary>
	///     The update Payload
	/// </summary>
	[DataMember(Name = "updatePayload")]
	public string UpdatePayload { get; set; }

	/// <summary>
	///     The clear Payload
	/// </summary>
	[DataMember(Name = "clearPayload")]
	public string ClearPayload { get; set; }

	/// <summary>
	///     The acknowledgement Payload
	/// </summary>
	[DataMember(Name = "ackPayload")]
	public string AckPayload { get; set; }

	/// <summary>
	///     The PayloadFormat
	/// </summary>
	[DataMember(Name = "payloadFormat")]
	public string PayloadFormat { get; set; }

	/// <summary>
	///     The update PayloadFormat
	/// </summary>
	[DataMember(Name = "updatePayloadFormat")]
	public string UpdatePayloadFormat { get; set; }

	/// <summary>
	///     The clear PayloadFormat
	/// </summary>
	[DataMember(Name = "clearPayloadFormat")]
	public string ClearPayloadFormat { get; set; }

	/// <summary>
	///     The acknowledgement PayloadFormat
	/// </summary>
	[DataMember(Name = "ackPayloadFormat")]
	public string AckPayloadFormat { get; set; }

	/// <summary>
	///     The Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public List<object> Headers { get; set; }

	/// <summary>
	///     The update Headers
	/// </summary>
	[DataMember(Name = "updateHeaders")]
	public List<object> UpdateHeaders { get; set; }

	/// <summary>
	///     The clear Headers
	/// </summary>
	[DataMember(Name = "clearHeaders")]
	public List<object> ClearHeaders { get; set; }

	/// <summary>
	///     The acknowledgement Headers
	/// </summary>
	[DataMember(Name = "ackHeaders")]
	public List<object> AckHeaders { get; set; }

	/// <summary>
	///     The parse method
	/// </summary>
	[DataMember(Name = "parseMethod")]
	public string ParseMethod { get; set; }

	/// <summary>
	///     The parse expression
	/// </summary>
	[DataMember(Name = "parseExpression")]
	public string ParseExpression { get; set; }

	/// <summary>
	///     The parse method
	/// </summary>
	[DataMember(Name = "enabledStatus")]
	public List<string> EnabledStatus { get; set; }
}
