namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
public class ResourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	public ResourceScheduledDownTimeCreationDto(int deviceId) : base(ScheduledDownTimeType.Resource)
	{
		DeviceId = deviceId;
	}

	/// <summary>
	/// The id of the device that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The name of the device that this SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceName { get; set; } = string.Empty;
}
