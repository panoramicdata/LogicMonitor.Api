namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource SDT creation DTO
/// </summary>
public class ResourceEventSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Resource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="deviceEventSourceId">Resource EventSource Id</param>
	public ResourceEventSourceScheduledDownTimeCreationDto(
		int resourceId,
		int deviceEventSourceId) : base(ScheduledDownTimeType.ResourceEventSource)
	{
		ResourceId = resourceId;
		ResourceEventSourceId = deviceEventSourceId;
	}

	/// <summary>
	///    Resource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="deviceEventSourceId"></param>
	public ResourceEventSourceScheduledDownTimeCreationDto(
		int resourceId,
		string deviceEventSourceId) : base(ScheduledDownTimeType.ResourceEventSource)
	{
		ResourceId = resourceId;
		EventSourceName = deviceEventSourceId;
	}

	/// <summary>
	///    The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	///    The ResourceDataSource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? ResourceEventSourceId { get; set; }

	/// <summary>
	///    The EventSource name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;
}
