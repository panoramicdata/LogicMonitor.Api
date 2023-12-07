namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Resource Property update DTO
/// </summary>
[DataContract]
public class ResourcePropertyUpdateDto : IHasSingletonEndpoint
{
	/// <summary>
	/// The resource's name
	/// </summary>
	[DataMember(Name = "resourceName")]
	public string ResourceName { get; set; } = string.Empty;

	/// <summary>
	/// Resource Ids
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public Dictionary<string, string> ResourceIds { get; set; } = [];

	/// <summary>
	/// Resource Properties
	/// </summary>
	[DataMember(Name = "resourceProperties")]
	public Dictionary<string, string> ResourceProperties { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "resource_property/ingest";
}
