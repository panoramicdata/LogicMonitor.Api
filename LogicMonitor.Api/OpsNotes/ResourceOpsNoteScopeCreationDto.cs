namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A device Ops Note Scope creation DTO
/// </summary>
[DataContract]
public class ResourceOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public ResourceOpsNoteScopeCreationDto()
	{
		Type = "device";
	}

	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
