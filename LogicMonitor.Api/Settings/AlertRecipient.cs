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
	public string Type { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; }

	/// <summary>
	/// The username/address
	/// </summary>
	[DataMember(Name = "addr")]
	public string UserName { get; set; }

	/// <summary>
	/// The contact details
	/// </summary>
	[DataMember(Name = "contact")]
	public string Contact { get; set; }
}
