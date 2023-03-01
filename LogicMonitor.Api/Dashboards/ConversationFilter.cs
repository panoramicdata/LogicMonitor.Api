namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Netflow filter conversation
/// </summary>
[DataContract]
public class ConversationFilter
{
	/// <summary>
	/// includeOrExclude
	/// </summary>
	[DataMember(Name = "includeOrExclude")]
	public string IncludeOrExclude { get; set; } = string.Empty;

	/// <summary>
	/// fromOperator
	/// </summary>
	[DataMember(Name = "fromOperator")]
	public string FromOperator { get; set; } = string.Empty;

	/// <summary>
	/// fromEndpoint
	/// </summary>
	[DataMember(Name = "fromEndpoint")]
	public string FromEndpoint { get; set; } = string.Empty;

	/// <summary>
	/// toOperator
	/// </summary>
	[DataMember(Name = "toOperator")]
	public string ToOperator { get; set; } = string.Empty;

	/// <summary>
	/// toEndpoint
	/// </summary>
	[DataMember(Name = "toEndpoint")]
	public string ToEndpoint { get; set; } = string.Empty;
}

