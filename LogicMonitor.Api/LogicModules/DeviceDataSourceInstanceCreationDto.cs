using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A device datasource instance creation DTO
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceCreationDto
{
	/// <summary>
	/// The datasource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; }

	/// <summary>
	/// The instance display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	/// The instance wild value
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; }

	/// <summary>
	/// The other instance wild value
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; }
}
