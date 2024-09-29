namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
public class DeviceDataSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId">DeviceDataSource Id</param>
	public DeviceDataSourceScheduledDownTimeCreationDto(
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
	public DeviceDataSourceScheduledDownTimeCreationDto(
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
	///    The device data source id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? DeviceDataSourceId { get; set; }

	/// <summary>
	///    The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;
}
