using System.Runtime.Serialization;

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
	public string Type { get; set; }

	/// <summary>
	/// The emailSubject
	/// </summary>
	[DataMember(Name = "emailSubject")]
	public string EmailSubject { get; set; }

	/// <summary>
	/// The emailBody
	/// </summary>
	[DataMember(Name = "emailBody")]
	public string EmailBody { get; set; }

	/// <summary>
	/// The smsSubject
	/// </summary>
	[DataMember(Name = "smsSubject")]
	public string SmsSubject { get; set; }

	/// <summary>
	/// The smsBody
	/// </summary>
	[DataMember(Name = "smsBody")]
	public string SmsBody { get; set; }

	/// <summary>
	/// The voice
	/// </summary>
	[DataMember(Name = "voice")]
	public string Voice { get; set; }
}
