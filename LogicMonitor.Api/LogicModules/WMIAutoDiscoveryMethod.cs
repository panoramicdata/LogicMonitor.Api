using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// WMIAutoDiscoveryMethod
/// </summary>

[DataContract]
public class WMIAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The linked classes
	/// </summary>
	[DataMember(Name = "linkedClasses", IsRequired = false)]
	public List<LinkedWmiClass>? LinkedClasses { get; set; }

	/// <summary>
	/// The externalResourceID
	/// </summary>
	[DataMember(Name = "externalResourceID", IsRequired = false)]
	public string? ExternalResourceID { get; set; }

	/// <summary>
	/// The externalResourceType
	/// </summary>
	[DataMember(Name = "externalResourceType", IsRequired = false)]
	public string? ExternalResourceType { get; set; }

	/// <summary>
	/// wmiClass
	/// </summary>
	[DataMember(Name = "wmiClass", IsRequired = true)]
	public string WmiClass { get; set; } = null!;

	/// <summary>
	/// property
	/// </summary>
	[DataMember(Name = "property", IsRequired = true)]
	public string Property { get; set; } = null!;

	/// <summary>
	/// namespace
	/// </summary>
	[DataMember(Name = "namespace", IsRequired = true)]
	public string Namespace { get; set; } = null!;

	/// <summary>
	/// ILP
	/// </summary>
	[DataMember(Name = "ILP", IsRequired = false)]
	public InstanceLevelProperty? ILP { get; set; }

	/// <summary>
	/// Whether to enabled linked class instance level properties
	/// </summary>
	[DataMember(Name = "enableLinkedClassILP", IsRequired = false)]
	public bool AreLinkedClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableWmiClassILP", IsRequired = false)]
	public bool AreWmiClassInstanceLevelPropertiesEnabled { get; set; }
}
