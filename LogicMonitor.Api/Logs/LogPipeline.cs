namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log pipeline (LogAlertGroup)
/// </summary>
[DataContract]
public class LogPipeline : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// Description of the log pipeline
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Name of the log pipeline
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Partitions of the log pipeline
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<string> Partitions { get; set; } = [];

	/// <summary>
	/// Query associated with the log pipeline
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <inheritdoc />
	public string Endpoint() => "logpipelines";
}
