namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource SDT creation DTO
/// </summary>
public class ResourceEventSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="deviceEventSourceId">DeviceEventSource Id</param>
	public ResourceEventSourceScheduledDownTimeCreationDto(
		int resourceId,
		int deviceEventSourceId) : base(ScheduledDownTimeType.DeviceEventSource)
	{
		DeviceId = resourceId;
		DeviceEventSourceId = deviceEventSourceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="deviceEventSourceId"></param>
	public ResourceEventSourceScheduledDownTimeCreationDto(
		int resourceId,
		string deviceEventSourceId) : base(ScheduledDownTimeType.DeviceEventSource)
	{
		DeviceId = resourceId;
		EventSourceName = deviceEventSourceId;
	}

	/// <summary>
	///    The Device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///    The ResourceDataSource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? DeviceEventSourceId { get; set; }

	/// <summary>
	///    The EventSource name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;
}
