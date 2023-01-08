namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    DeviceGroup SDT creation DTO
/// </summary>
public class ResourceGroupScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Device
	/// </summary>
	/// <param name="deviceGroupId"></param>
	public ResourceGroupScheduledDownTimeCreationDto(int deviceGroupId) : base(ScheduledDownTimeType.ResourceGroup)
	{
		DeviceGroupId = deviceGroupId;
	}

	/// <summary>
	///    The DeviceGroup id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///    The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }

	/// <summary>
	///    The DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; }
}
