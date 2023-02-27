namespace LogicMonitor.Api.Collectors;

/// <summary>
/// PaaSMongoDbCollectorAttribute
/// </summary>

[DataContract]
public class PaaSMongoDbCollectorAttribute : AttributeCollector
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "resourceUrl")]
	public string ResourceUrl { get; set; }
}
