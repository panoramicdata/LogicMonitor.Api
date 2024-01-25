namespace LogicMonitor.Api.Settings;

/// <summary>
/// An external alert destination
/// </summary>
[DataContract]
public class RecipientGroupCreationDto : CreationDto<RecipientGroup>, IHasName, IHasDescription
{
	/// <summary>
	/// The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The recipients
	/// </summary>
	[DataMember(Name = "recipients")]
	public List<AlertRecipient> Recipients { get; set; } = [];
}