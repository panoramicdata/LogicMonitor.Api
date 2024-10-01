namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A Resource datasource instance creation DTO
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceCreationDto
{
	/// <summary>
	/// The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The instance display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The instance wild value
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; } = string.Empty;

	/// <summary>
	/// The other instance wild value
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; } = string.Empty;
}
