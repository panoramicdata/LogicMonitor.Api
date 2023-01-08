namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
public class DeviceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	public DeviceScheduledDownTimeCreationDto(int deviceId) : base(ScheduledDownTimeType.Resource)
	{
		DeviceId = deviceId;
	}

	/// <summary>
	///    The collector id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
