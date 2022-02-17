namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device SDT creation DTO
/// </summary>
public class DeviceDataSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId">DeviceDataSource Id</param>
	public DeviceDataSourceScheduledDownTimeCreationDto(int deviceId, int deviceDataSourceId) : base(ScheduledDownTimeType.DeviceDataSource)
	{
		DeviceId = deviceId;
		DeviceDataSourceId = deviceDataSourceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="dataSourceName"></param>
	public DeviceDataSourceScheduledDownTimeCreationDto(int deviceId, string dataSourceName) : base(ScheduledDownTimeType.DeviceDataSource)
	{
		DeviceId = deviceId;
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
	public string? DataSourceName { get; set; }
}
