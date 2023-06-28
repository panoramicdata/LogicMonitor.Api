namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
public class DeviceEventSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceEventSourceId">DeviceEventSource Id</param>
	public DeviceEventSourceScheduledDownTimeCreationDto(int deviceId, int deviceEventSourceId) : base(ScheduledDownTimeType.DeviceEventSource)
	{
		DeviceId = deviceId;
		DeviceEventSourceId = deviceEventSourceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceEventSourceId"></param>
	public DeviceEventSourceScheduledDownTimeCreationDto(int deviceId, string deviceEventSourceId) : base(ScheduledDownTimeType.DeviceEventSource)
	{
		DeviceId = deviceId;
		EventSourceName = deviceEventSourceId;
	}

	/// <summary>
	///    The Device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///    The device data source id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? DeviceEventSourceId { get; set; }

	/// <summary>
	///    The EventSource name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;
}
