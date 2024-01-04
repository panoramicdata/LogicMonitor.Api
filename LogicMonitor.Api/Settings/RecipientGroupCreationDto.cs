namespace LogicMonitor.Api.Settings;

/// <summary>
/// An external alert destination
/// </summary>
[DataContract]
public class RecipientGroupCreationDto : CreationDto<RecipientGroup>
{
	/// <summary>
	/// The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

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