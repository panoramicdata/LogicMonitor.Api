namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    DeviceGroup SDT creation DTO
/// </summary>
/// <remarks>
///    Device
/// </remarks>
/// <param name="deviceGroupId"></param>
public class ResourceGroupScheduledDownTimeCreationDto(int deviceGroupId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.ResourceGroup)
{

	/// <summary>
	/// The id of the device group that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; } = deviceGroupId;

	/// <summary>
	/// The id of the datasource that this SDT will be associated with, for the specified group. dataSourceId 0 indicates all datasources
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The name of the datasource that this SDT will be associated with, for the specified group. dataSourceName \"All\" indicates all datasources
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The full path of the device group that this SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; } = string.Empty;
}
