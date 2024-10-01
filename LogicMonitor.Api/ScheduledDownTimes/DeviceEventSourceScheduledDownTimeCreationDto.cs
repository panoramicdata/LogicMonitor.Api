namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Obsolete
/// </summary>
/// <param name="resourceId"></param>
/// <param name="deviceEventSourceId"></param>
[Obsolete("Use ResourceEventSourceScheduledDownTimeCreationDto instead", true)]
public class DeviceEventSourceScheduledDownTimeCreationDto(int resourceId, int deviceEventSourceId)
	: ResourceEventSourceScheduledDownTimeCreationDto(resourceId, deviceEventSourceId);
