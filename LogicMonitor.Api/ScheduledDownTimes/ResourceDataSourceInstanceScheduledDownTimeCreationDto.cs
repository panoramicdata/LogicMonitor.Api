namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource DataSource Instance SDT creation DTO
/// </summary>
public class ResourceDataSourceInstanceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceInstanceId"></param>
	public ResourceDataSourceInstanceScheduledDownTimeCreationDto(
		int resourceId,
		int dataSourceInstanceId) : base(ScheduledDownTimeType.DeviceDataSourceInstance)
	{
		DeviceId = resourceId;
		DataSourceInstanceId = dataSourceInstanceId;
	}

	/// <summary>
	///    Device
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceInstanceName"></param>
	public ResourceDataSourceInstanceScheduledDownTimeCreationDto(
		int resourceId,
		string dataSourceInstanceName) : base(ScheduledDownTimeType.DeviceDataSourceInstance)
	{
		DeviceId = resourceId;
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
	public string DataSourceInstanceName { get; set; } = string.Empty;
}
