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
	[DataMember(Name = "attribute", IsRequired = true)]
	public string Attribute { get; set; } = null!;

	/// <summary>
	/// filter\u0027s operation values can be : Equal|NotEqual|GreaterThan|GreaterEqual|LessThan|LessEqual|Contain|NotContain|NotExist|RegexMatch|RegexNotMatch
	/// </summary>
	[DataMember(Name = "operation", IsRequired = true)]
	public AutoDiscoveryFilterOperation Operation { get; set; }

	/// <summary>
	/// operation value
	/// </summary>
	[DataMember(Name = "value", IsRequired = false)]
	public string? Value { get; set; }

	/// <summary>
	/// filter comment
	/// </summary>
	[DataMember(Name = "comment", IsRequired = false)]
	public string? Comment { get; set; }
}
