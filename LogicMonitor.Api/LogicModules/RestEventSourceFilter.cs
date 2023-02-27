namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource filter
/// </summary>
[DataContract]
public class RestEventSourceFilter : UndescribedNamedItem
{
	/// <summary>
	/// filter operator
	/// </summary>
	[DataMember(Name = "operator")]
	public string Operator { get; set; } = null!;

	/// <summary>
	/// filter value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = null!;

	/// <summary>
	/// filter comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = null!;
}
