namespace LogicMonitor.Api.Settings;

/// <summary>
///     An email integration
/// </summary>
[DataContract]
public class EmailIntegration : Integration
{
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
