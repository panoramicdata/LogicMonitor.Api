namespace LogicMonitor.Api.Settings;

/// <summary>
/// An alert recipient
/// </summary>
[DataContract]
public class AlertRecipient
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	/// The username/address
	/// </summary>
	[DataMember(Name = "addr")]
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	/// The contact details
	/// </summary>
	[DataMember(Name = "contact")]
	public string Contact { get; set; } = string.Empty;
}
