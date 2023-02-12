using System;
using System.Collections.Generic;
using System.Text;

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
	[DataMember(Name = "valueAttribute", IsRequired = true)]
	public string ValueAttribute { get; set; } = null!;

	/// <summary>
	/// aliasAttribute
	/// </summary>
	[DataMember(Name = "aliasAttribute", IsRequired = false)]
	public string AliasAttribute { get; set; } = string.Empty;

	/// <summary>
	/// descriptionAttribute
	/// </summary>
	[DataMember(Name = "descriptionAttribute", IsRequired = false)]
	public string DescriptionAttribute { get; set; } = string.Empty;

	/// <summary>
	/// resourceUrl
	/// </summary>
	[DataMember(Name = "resourceUrl", IsRequired = true)]
	public string ResourceUrl { get; set; } = null!;

	/// <summary>
	/// propertyAttributes
	/// </summary>
	[DataMember(Name = "propertyAttributes", IsRequired = false)]
	public string PropertyAttributes { get; set; } = string.Empty;

	/// <summary>
	/// value2Attribute
	/// </summary>
	[DataMember(Name = "value2Attribute", IsRequired = false)]
	public string Value2Attribute { get; set; } = string.Empty;

	/// <summary>
	/// instancePath
	/// </summary>
	[DataMember(Name = "instancePath", IsRequired = true)]
	public string InstancePath { get; set; } = null!;
}
