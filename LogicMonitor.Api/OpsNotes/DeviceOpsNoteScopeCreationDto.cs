namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A device Ops Note Scope creation DTO
/// </summary>
[DataContract]
public class DeviceOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public DeviceOpsNoteScopeCreationDto()
	{
		Type = "device";
	}

	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
