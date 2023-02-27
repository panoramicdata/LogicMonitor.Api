namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// PaaSJsonPathDiscoveryMethod
/// </summary>

[DataContract]
public class PaaSJsonPathDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// valueAttribute
	/// </summary>
	[DataMember(Name = "valueAttribute")]
	public string ValueAttribute { get; set; } = null!;

	/// <summary>
	/// aliasAttribute
	/// </summary>
	[DataMember(Name = "aliasAttribute")]
	public string AliasAttribute { get; set; } = string.Empty;

	/// <summary>
	/// descriptionAttribute
	/// </summary>
	[DataMember(Name = "descriptionAttribute")]
	public string DescriptionAttribute { get; set; } = string.Empty;

	/// <summary>
	/// resourceUrl
	/// </summary>
	[DataMember(Name = "resourceUrl")]
	public string ResourceUrl { get; set; } = null!;

	/// <summary>
	/// propertyAttributes
	/// </summary>
	[DataMember(Name = "propertyAttributes")]
	public string PropertyAttributes { get; set; } = string.Empty;

	/// <summary>
	/// value2Attribute
	/// </summary>
	[DataMember(Name = "value2Attribute")]
	public string Value2Attribute { get; set; } = string.Empty;

	/// <summary>
	/// instancePath
	/// </summary>
	[DataMember(Name = "instancePath")]
	public string InstancePath { get; set; } = null!;
}
