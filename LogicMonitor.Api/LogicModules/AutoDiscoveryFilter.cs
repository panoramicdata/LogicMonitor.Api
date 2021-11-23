namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery filter
/// </summary>
[DataContract]
public class AutoDiscoveryFilter
{
	/// <summary>
	/// attribute
	/// </summary>
	[DataMember(Name = "attribute")]
	public string Attribute { get; set; }

	/// <summary>
	/// operation
	/// </summary>
	[DataMember(Name = "operation")]
	public string Operation { get; set; }

	/// <summary>
	/// value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }

	/// <summary>
	/// comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; }
}
