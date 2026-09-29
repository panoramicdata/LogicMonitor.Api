namespace LogicMonitor.Api.Reports;

/// <summary>
/// A resource or instance property filter on a report
/// </summary>
[DataContract]
public class PropertyFilter
{
	/// <summary>
	/// The rules the filter applies
	/// </summary>
	[DataMember(Name = "dynamic")]
	public List<PropertyFilterRule> Dynamic { get; set; } = [];

	/// <summary>
	/// The selected values. Only ever seen empty, so the element type is not assumed.
	/// </summary>
	[DataMember(Name = "selectedValues")]
	public List<object> SelectedValues { get; set; } = [];

	/// <summary>
	/// The property key. Only ever seen null, so the type is not assumed.
	/// </summary>
	[DataMember(Name = "propertyKey")]
	public object? PropertyKey { get; set; }
}
