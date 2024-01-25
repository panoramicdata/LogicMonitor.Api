namespace LogicMonitor.Api.Settings;

/// <summary>
/// HTTP Integration Creation Dto
/// </summary>
[DataContract]
public class HttpIntegrationCreationDto : IntegrationCreationDto<HttpIntegration>
{
	/// <summary>
	/// Constructor
	/// </summary>
	public HttpIntegrationCreationDto() : base("http")
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	internal HttpIntegrationCreationDto(string type) : base(type)
	{
	}

	/// <summary>
	///     The acknowledgement Headers
	/// </summary>
	[DataMember(Name = "ackHeaders")]
	public List<object> AckHeaders { get; set; } = [];

	/// <summary>
	///     The acknowledgement method
	/// </summary>
	[DataMember(Name = "ackMethod")]
	public string AckMethod { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth version
	/// </summary>
	[DataMember(Name = "ackOAuthVersion")]
	public string? AckOAuthVersion { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth grant type
	/// </summary>
	[DataMember(Name = "ackOAuthGrantType")]
	public string? AckOAuthGrantType { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth access token Url
	/// </summary>
	[DataMember(Name = "ackOAuthAccessTokenUrl")]
	public string? AckOAuthAccessTokenUrl { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth client id
	/// </summary>
	[DataMember(Name = "ackOAuthClientId")]
	public string? AckOAuthClientId { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth client secret
	/// </summary>
	[DataMember(Name = "ackOAuthClientSecret")]
	public string? AckOAuthClientSecret { get; set; } = string.Empty;

	/// <summary>
	///     The ACK OAuth scope
	/// </summary>
	[DataMember(Name = "ackOAuthScope")]
	public string? AckOAuthScope { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Password
	/// </summary>
	[DataMember(Name = "ackPassword")]
	public string AckPassword { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Payload
	/// </summary>
	[DataMember(Name = "ackPayload")]
	public string AckPayload { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement PayloadFormat
	/// </summary>
	[DataMember(Name = "ackPayloadFormat")]
	public string AckPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Url
	/// </summary>
	[DataMember(Name = "ackUrl")]
	public string AckUrl { get; set; } = string.Empty;

	/// <summary>
	///     The acknowledgement Username
	/// </summary>
	[DataMember(Name = "ackUsername")]
	public string AckUsername { get; set; } = string.Empty;

	/// <summary>
	///     The clear Headers
	/// </summary>
	[DataMember(Name = "clearHeaders")]
	public List<object> ClearHeaders { get; set; } = [];

	/// <summary>
	///     The clear method
	/// </summary>
	[DataMember(Name = "clearMethod")]
	public string ClearMethod { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth version
	/// </summary>
	[DataMember(Name = "clearOAuthVersion")]
	public string? ClearOAuthVersion { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth grant type
	/// </summary>
	[DataMember(Name = "clearOAuthGrantType")]
	public string? ClearOAuthGrantType { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth access token Url
	/// </summary>
	[DataMember(Name = "clearOAuthAccessTokenUrl")]
	public string? ClearOAuthAccessTokenUrl { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth client id
	/// </summary>
	[DataMember(Name = "clearOAuthClientId")]
	public string? ClearOAuthClientId { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth client secret
	/// </summary>
	[DataMember(Name = "clearOAuthClientSecret")]
	public string? ClearOAuthClientSecret { get; set; } = string.Empty;

	/// <summary>
	///     The Clear OAuth scope
	/// </summary>
	[DataMember(Name = "clearOAuthScope")]
	public string? ClearOAuthScope { get; set; } = string.Empty;

	/// <summary>
	///     The clear Password
	/// </summary>
	[DataMember(Name = "clearPassword")]
	public string ClearPassword { get; set; } = string.Empty;

	/// <summary>
	///     The clear Payload
	/// </summary>
	[DataMember(Name = "clearPayload")]
	public string ClearPayload { get; set; } = string.Empty;

	/// <summary>
	///     The clear PayloadFormat
	/// </summary>
	[DataMember(Name = "clearPayloadFormat")]
	public string ClearPayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The clear Url
	/// </summary>
	[DataMember(Name = "clearUrl")]
	public string ClearUrl { get; set; } = string.Empty;

	/// <summary>
	///     The clear Username
	/// </summary>
	[DataMember(Name = "clearUsername")]
	public string ClearUsername { get; set; } = string.Empty;

	/// <summary>
	///     The parse method
	/// </summary>
	[DataMember(Name = "enabledStatus")]
	public List<string> EnabledStatus { get; set; } = [];

	/// <summary>
	///     The Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public List<object> Headers { get; set; } = [];

	/// <summary>
	///     The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

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
	///     The Payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string Payload { get; set; } = string.Empty;

	/// <summary>
	///     The PayloadFormat
	/// </summary>
	[DataMember(Name = "payloadFormat")]
	public string PayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The update method
	/// </summary>
	[DataMember(Name = "updateMethod")]
	public string UpdateMethod { get; set; } = string.Empty;

	/// <summary>
	///     The update Url
	/// </summary>
	[DataMember(Name = "updateUrl")]
	public string UpdateUrl { get; set; } = string.Empty;

	/// <summary>
	///     The Url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	///     The Username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth version
	/// </summary>
	[DataMember(Name = "oAuthVersion")]
	public string? OAuthVersion { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth grant type
	/// </summary>
	[DataMember(Name = "oAuthGrantType")]
	public string? OAuthGrantType { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth access token Url
	/// </summary>
	[DataMember(Name = "oAuthAccessTokenUrl")]
	public string? OAuthAccessTokenUrl { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth client id
	/// </summary>
	[DataMember(Name = "oAuthClientId")]
	public string? OAuthClientId { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth client secret
	/// </summary>
	[DataMember(Name = "oAuthClientSecret")]
	public string? OAuthClientSecret { get; set; } = string.Empty;

	/// <summary>
	///     The OAuth scope
	/// </summary>
	[DataMember(Name = "oAuthScope")]
	public string? OAuthScope { get; set; } = string.Empty;

	/// <summary>
	///     The Password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	///     The update Headers
	/// </summary>
	[DataMember(Name = "updateHeaders")]
	public List<object> UpdateHeaders { get; set; } = [];

	/// <summary>
	///     The update Password
	/// </summary>
	[DataMember(Name = "updatePassword")]
	public string UpdatePassword { get; set; } = string.Empty;

	/// <summary>
	///     The update Payload
	/// </summary>
	[DataMember(Name = "updatePayload")]
	public string UpdatePayload { get; set; } = string.Empty;

	/// <summary>
	///     The update PayloadFormat
	/// </summary>
	[DataMember(Name = "updatePayloadFormat")]
	public string UpdatePayloadFormat { get; set; } = string.Empty;

	/// <summary>
	///     The update Username
	/// </summary>
	[DataMember(Name = "updateUsername")]
	public string UpdateUsername { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth version
	/// </summary>
	[DataMember(Name = "updateOAuthVersion")]
	public string? UpdateOAuthVersion { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth grant type
	/// </summary>
	[DataMember(Name = "updateOAuthGrantType")]
	public string? UpdateOAuthGrantType { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth access token Url
	/// </summary>
	[DataMember(Name = "updateOAuthAccessTokenUrl")]
	public string? UpdateOAuthAccessTokenUrl { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth client id
	/// </summary>
	[DataMember(Name = "updateOAuthClientId")]
	public string? UpdateOAuthClientId { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth client secret
	/// </summary>
	[DataMember(Name = "updateOAuthClientSecret")]
	public string? UpdateOAuthClientSecret { get; set; } = string.Empty;

	/// <summary>
	///     The Update OAuth scope
	/// </summary>
	[DataMember(Name = "updateOAuthScope")]
	public string? UpdateOAuthScope { get; set; } = string.Empty;

	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;
}
