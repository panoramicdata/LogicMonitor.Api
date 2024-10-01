namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Obsolete
/// </summary>
/// <param name="resourceId"></param>
/// <param name="dataSourceInstanceId"></param>
[Obsolete("Use ResourceDataSourceInstanceScheduledDownTimeCreationDto instead", true)]
public class DeviceDataSourceInstanceScheduledDownTimeCreationDto(int resourceId, int dataSourceInstanceId)
	: ResourceDataSourceInstanceScheduledDownTimeCreationDto(resourceId, dataSourceInstanceId);
