namespace LogicMonitor.Api.Settings;

/// <summary>
/// An external alert destination
/// </summary>
[DataContract]
public class RecipientGroup : DescribedItem, IHasEndpoint, IHasName
{
	/// <summary>
	/// The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The recipients
	/// </summary>
	[DataMember(Name = "recipients")]
	public List<AlertRecipient> Recipients { get; set; } = [];

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/recipientgroups";
}
