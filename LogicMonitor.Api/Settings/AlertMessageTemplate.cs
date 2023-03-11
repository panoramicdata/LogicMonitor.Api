namespace LogicMonitor.Api.Settings;

/// <summary>
/// An alert message template
/// </summary>
[DataContract]
public class AlertMessageTemplate
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The emailSubject
	/// </summary>
	[DataMember(Name = "emailSubject")]
	public string EmailSubject { get; set; } = string.Empty;

	/// <summary>
	/// The emailBody
	/// </summary>
	[DataMember(Name = "emailBody")]
	public string EmailBody { get; set; } = string.Empty;

	/// <summary>
	/// The smsSubject
	/// </summary>
	[DataMember(Name = "smsSubject")]
	public string SmsSubject { get; set; } = string.Empty;

	/// <summary>
	/// The smsBody
	/// </summary>
	[DataMember(Name = "smsBody")]
	public string SmsBody { get; set; } = string.Empty;

	/// <summary>
	/// The voice
	/// </summary>
	[DataMember(Name = "voice")]
	public string Voice { get; set; } = string.Empty;
}
