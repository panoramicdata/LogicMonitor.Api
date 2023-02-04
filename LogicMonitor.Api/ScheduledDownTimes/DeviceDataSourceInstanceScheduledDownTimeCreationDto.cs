namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Device DataSource Instance SDT creation DTO
/// </summary>
public class DeviceDataSourceInstanceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="dataSourceInstanceId"></param>
	public DeviceDataSourceInstanceScheduledDownTimeCreationDto(int deviceId, int dataSourceInstanceId) : base(ScheduledDownTimeType.DeviceDataSourceInstance)
	{
		DeviceId = deviceId;
		DataSourceInstanceId = dataSourceInstanceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="dataSourceInstanceName"></param>
	public DeviceDataSourceInstanceScheduledDownTimeCreationDto(int deviceId, string dataSourceInstanceName) : base(ScheduledDownTimeType.DeviceDataSourceInstance)
	{
		DeviceId = deviceId;
		DataSourceInstanceName = dataSourceInstanceName;
	}

	/// <summary>
	///    The Device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///    The data source id
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int? DataSourceInstanceId { get; set; }

	/// <summary>
	///    The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceInstanceName")]
	public string DataSourceInstanceName { get; set; }
}
