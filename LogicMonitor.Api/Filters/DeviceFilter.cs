namespace LogicMonitor.Api.Filters;

/// <summary>
/// DeviceFilter
/// </summary>
[DataContract]
public class DeviceFilter
{
	/// <summary>
	/// comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// attribute
	/// </summary>
	[DataMember(Name = "attribute")]
	public string Attribute { get; set; } = string.Empty;

	/// <summary>
	/// operation
	/// </summary>
	[DataMember(Name = "operation")]
	public string Operation { get; set; } = string.Empty;

	/// <summary>
	/// value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
