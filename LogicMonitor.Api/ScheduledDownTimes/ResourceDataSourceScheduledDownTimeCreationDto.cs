namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource SDT creation DTO
/// </summary>
public class ResourceDataSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId">DeviceDataSource Id</param>
	public ResourceDataSourceScheduledDownTimeCreationDto(
		int resourceId,
		int resourceDataSourceId) : base(ScheduledDownTimeType.DeviceDataSource)
	{
		DeviceId = resourceId;
		DeviceDataSourceId = resourceDataSourceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceName"></param>
	public ResourceDataSourceScheduledDownTimeCreationDto(
		int resourceId,
		string dataSourceName) : base(ScheduledDownTimeType.DeviceDataSource)
	{
		DeviceId = resourceId;
		DataSourceName = dataSourceName;
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
	public int? DeviceDataSourceId { get; set; }

	/// <summary>
	///    The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;
}
