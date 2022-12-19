namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A device group Ops Note Scope creation DTO
/// </summary>
[DataContract]
public class DeviceGroupOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public DeviceGroupOpsNoteScopeCreationDto()
	{
		Type = "deviceGroup";
	}

	/// <summary>
	/// The device Id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int DeviceGroupId { get; set; }
}
