namespace LogicMonitor.Api.Settings;

/// <summary>
/// Email Integration Creation Dto
/// </summary>
[DataContract]
public class EmailIntegrationCreationDto : IntegrationCreationDto<EmailIntegration>
{
	/// <summary>
	/// Constructor
	/// </summary>
	public EmailIntegrationCreationDto() : base("http")
	{
	}
	/// <summary>
	///     The sender
	/// </summary>
	[DataMember(Name = "sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	///     The receivers
	/// </summary>
	[DataMember(Name = "receivers")]
	public string Receivers { get; set; } = string.Empty;

	/// <summary>
	///     The subject
	/// </summary>
	[DataMember(Name = "subject")]
	public string Subject { get; set; } = string.Empty;

	/// <summary>
	///     The body
	/// </summary>
	[DataMember(Name = "body")]
	public string Body { get; set; } = string.Empty;
}
