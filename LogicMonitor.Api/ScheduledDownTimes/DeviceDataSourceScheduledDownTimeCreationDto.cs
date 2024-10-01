namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Obsolete
/// </summary>
[Obsolete("Use ResourceDataSourceScheduledDownTimeCreationDto instead", true)]
public class DeviceDataSourceScheduledDownTimeCreationDto(int resourceId, int resourceDataSourceId)
	: ResourceDataSourceScheduledDownTimeCreationDto(resourceId, resourceDataSourceId);
