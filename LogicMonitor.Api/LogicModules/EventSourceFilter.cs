namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource filter
/// </summary>
[DataContract]
public class EventSourceFilter : UndescribedNamedItem
{
	/// <summary>
	/// The operator
	/// </summary>
	[DataMember(Name = "operator")]
	public string Operator { get; set; }

	/// <summary>
	/// The value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }

	/// <summary>
	/// The comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; }
}
