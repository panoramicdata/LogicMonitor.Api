namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
/// <remarks>
///    Device
/// </remarks>
/// <param name="deviceId"></param>
public class ResourceScheduledDownTimeCreationDto(int deviceId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Resource)
{

	/// <summary>
	/// The id of the device that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; } = deviceId;

	/// <summary>
	/// The name of the device that this SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceName { get; set; } = string.Empty;
}
