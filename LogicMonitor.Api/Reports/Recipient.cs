namespace LogicMonitor.Api.Reports;

/// <summary>
/// Recipient
/// </summary>

[DataContract]
public class Recipient
{
	/// <summary>
	/// EMAIL|SMEMAIL|VOICE|SMS, Recipient method for each type\n            group: \"method\" not used\n            arbitrary: the method should be email.\n            admin: the method  Should be email, smsEmail, voice, sms, or defaultMethod;\n
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } 

	/// <summary>
	/// contact details, email address or phone number
	/// </summary>
	[DataMember(Name = "contact")]
	public string? Contact { get; set; }

	/// <summary>
	/// GROUP|ARBITRARY|ADMIN, where Admin \u003d a user, and Arbitrary \u003d an arbitrary email
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } 

	/// <summary>
	/// the user name if method \u003d admin, or the email address if method \u003d arbitrary
	/// </summary>
	[DataMember(Name = "addr")]
	public string? Addr { get; set; }
}
