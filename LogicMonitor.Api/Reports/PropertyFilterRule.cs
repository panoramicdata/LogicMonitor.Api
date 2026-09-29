namespace LogicMonitor.Api.Reports;

/// <summary>
/// A rule in a report property filter, comparing one property with one or more expressions
/// </summary>
[DataContract]
public class PropertyFilterRule
{
	/// <summary>
	/// The mode, for example include
	/// </summary>
	[DataMember(Name = "mode")]
	public string Mode { get; set; } = string.Empty;

	/// <summary>
	/// The property name, for example system.categories
	/// </summary>
	[DataMember(Name = "field")]
	public string Field { get; set; } = string.Empty;

	/// <summary>
	/// The kind of property, for example RESOURCE_PROPERTY
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The expressions the property is compared with
	/// </summary>
	[DataMember(Name = "expressions")]
	public List<PropertyFilterExpression> Expressions { get; set; } = [];
}
