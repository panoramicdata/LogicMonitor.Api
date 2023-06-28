namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Resource Property update DTO
/// Supports put and patch only
/// </summary>
[DataContract]
public class InstancePropertyUpdateDto : IHasSingletonEndpoint
{
	/// <summary>
	/// Resource Ids
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public Dictionary<string, string> ResourceIds { get; set; } = new();

	/// <summary>
	/// DataSource unique name
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; } = string.Empty;

	/// <summary>
	/// DataSource Display Name
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Instance Name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	/// Instance Properties
	/// </summary>
	[DataMember(Name = "instanceProperties")]
	public Dictionary<string, string> InstanceProperties { get; set; } = new();

	/// <inheritdoc />
	public string Endpoint() => "instance_property/ingest";
}
