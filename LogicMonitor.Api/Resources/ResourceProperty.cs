namespace LogicMonitor.Api.Resources;

/// <summary>
/// Device Property
/// </summary>

[DataContract]
public class ResourceProperty
{
	/// <summary>
	/// The resource property name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The resource property value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
