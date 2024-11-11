namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource SDT creation DTO
/// </summary>
public class ResourceDataSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Resource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId">Resource DataSource Id</param>
	public ResourceDataSourceScheduledDownTimeCreationDto(
		int resourceId,
		int resourceDataSourceId) : base(ScheduledDownTimeType.ResourceDataSource)
	{
		ResourceId = resourceId;
		ResourceDataSourceId = resourceDataSourceId;
	}

	/// <summary>
	///    Resource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceName"></param>
	public ResourceDataSourceScheduledDownTimeCreationDto(
		int resourceId,
		string dataSourceName) : base(ScheduledDownTimeType.ResourceDataSource)
	{
		ResourceId = resourceId;
		DataSourceName = dataSourceName;
	}

	/// <summary>
	///    The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	///    The ResourceDataSource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? ResourceDataSourceId { get; set; }

	/// <summary>
	///    The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;
}
