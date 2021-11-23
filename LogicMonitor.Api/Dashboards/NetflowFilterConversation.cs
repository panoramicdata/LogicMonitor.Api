using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Netflow filter conversation
/// </summary>
[DataContract]
public class NetflowFilterConversation
{
	/// <summary>
	/// includeOrExclude
	/// </summary>
	[DataMember(Name = "includeOrExclude")]
	public string IncludeOrExclude { get; set; }

	/// <summary>
	/// fromOperator
	/// </summary>
	[DataMember(Name = "fromOperator")]
	public string FromOperator { get; set; }

	/// <summary>
	/// fromEndpoint
	/// </summary>
	[DataMember(Name = "fromEndpoint")]
	public string FromEndpoint { get; set; }

	/// <summary>
	/// toOperator
	/// </summary>
	[DataMember(Name = "toOperator")]
	public string ToOperator { get; set; }

	/// <summary>
	/// toEndpoint
	/// </summary>
	[DataMember(Name = "toEndpoint")]
	public string ToEndpoint { get; set; }
}

