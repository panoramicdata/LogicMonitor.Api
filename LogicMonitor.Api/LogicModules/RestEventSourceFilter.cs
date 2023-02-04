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
	[DataMember(Name = "operator", IsRequired = true)]
	public string Operator { get; set; } = null!;

	/// <summary>
	/// filter value
	/// </summary>
	[DataMember(Name = "value", IsRequired = true)]
	public string Value { get; set; } = null!;

	/// <summary>
	/// filter comment
	/// </summary>
	[DataMember(Name = "comment", IsRequired = true)]
	public string Comment { get; set; } = null!;
}
