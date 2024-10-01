namespace LogicMonitor.Api.Resources;

/// <summary>
/// Resource with DataSource instance information
/// </summary>
[DataContract]
public class ResourceWithDataSourceInstanceInformation : Resource
{
	/// <summary>
	/// The DataSourceInstance information
	/// </summary>
	[DataMember(Name = "instance")]
	public List<ResourceDataSourceInstanceSummary> DeviceDataSourceInstances { get; set; } = [];
}
