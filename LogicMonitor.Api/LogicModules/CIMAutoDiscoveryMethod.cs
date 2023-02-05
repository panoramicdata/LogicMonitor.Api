using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// CIMAutoDiscoveryMethod
/// </summary>

[DataContract]
public class CIMAutoDiscoveryMethod
{
	/// <summary>
	/// The CIM class
	/// </summary>
	[DataMember(Name = "cimClass", IsRequired = true)]
	public string CimClass { get; set; } = null!;

	/// <summary>
	/// The property
	/// </summary>
	[DataMember(Name = "property", IsRequired = true)]
	public string Property { get; set; } = null!;

	/// <summary>
	/// The namespace
	/// </summary>
	[DataMember(Name = "namespace", IsRequired = true)]
	public string Namespace { get; set; } = null!;
}
