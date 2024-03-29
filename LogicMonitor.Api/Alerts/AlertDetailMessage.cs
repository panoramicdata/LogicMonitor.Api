namespace LogicMonitor.Api.Alerts;

/// <summary>
///    An Alert Detail message
/// </summary>
[DataContract]
public class AlertDetailMessage
{
	/// <summary>
	///    The subject
	/// </summary>
	[DataMember(Name = "subject")]
	public string Subject { get; set; } = string.Empty;

	/// <summary>
	///    The body
	/// </summary>
	[DataMember(Name = "body")]
	public string Body { get; set; } = string.Empty;
}
