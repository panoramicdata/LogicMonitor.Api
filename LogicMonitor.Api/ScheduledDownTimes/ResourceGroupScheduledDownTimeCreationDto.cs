namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    ResourceGroup SDT creation DTO
/// </summary>
/// <remarks>
///    Resource
/// </remarks>
/// <param name="resourceGroupId"></param>
public class ResourceGroupScheduledDownTimeCreationDto(int resourceGroupId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.ResourceGroup)
{

	/// <summary>
	/// The id of the ResourceGroup that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; } = resourceGroupId;

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
	/// The full path of the ResourceGroup that this SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;
}
