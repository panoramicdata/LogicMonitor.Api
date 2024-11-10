namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A ResourceGroup Ops Note Scope creation DTO
/// </summary>
[DataContract]
public class ResourceGroupOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public ResourceGroupOpsNoteScopeCreationDto()
	{
		Type = "deviceGroup";
	}

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int ResourceGroupId { get; set; }
}
