namespace LogicMonitor.Api.Settings;

/// <summary>
///     New user message template
/// </summary>
[DataContract]
public class NewUserMessageTemplate : IHasSingletonEndpoint
{
	/// <summary>
	///     messageSubject
	/// </summary>
	[DataMember(Name = "messageSubject")]
	public string Subject { get; set; } = string.Empty;

	/// <summary>
	///     messageBody
	/// </summary>
	[DataMember(Name = "messageBody")]
	public string Body { get; set; } = string.Empty;

	/// <inheritdoc />
	public string Endpoint() => "setting/messagetemplate";
}
