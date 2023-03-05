namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery filter
/// </summary>
[DataContract]
public class AutoDiscoveryFilter
{
	/// <summary>
	/// attribute to filter
	/// </summary>
	[DataMember(Name = "attribute")]
	public string Attribute { get; set; } = string.Empty;

	/// <summary>
	/// filter\u0027s operation values can be : Equal|NotEqual|GreaterThan|GreaterEqual|LessThan|LessEqual|Contain|NotContain|NotExist|RegexMatch|RegexNotMatch
	/// </summary>
	[DataMember(Name = "operation")]
	public AutoDiscoveryFilterOperation Operation { get; set; }

	/// <summary>
	/// operation value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// filter comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;
}
