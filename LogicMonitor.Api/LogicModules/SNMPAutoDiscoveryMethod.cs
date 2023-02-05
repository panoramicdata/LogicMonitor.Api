using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// SNMPAutoDiscoveryMethod
/// </summary>

[DataContract]
public class SNMPAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableSNMPILP", IsRequired = false)]
	public bool EnableSNMPILP { get; set; }

	/// <summary>
	/// Lookup OID
	/// </summary>
	[DataMember(Name = "lookupOID", IsRequired = true)]
	public string LookupOid { get; set; } = null!;

	/// <summary>
	/// The external resource id
	/// </summary>
	[DataMember(Name = "externalResourceID", IsRequired = false)]
	public string? ExternalResourceId { get; set; }

	/// <summary>
	/// Description OID
	/// </summary>
	[DataMember(Name = "descriptionOID", IsRequired = false)]
	public string? DescriptionOid { get; set; }

	/// <summary>
	/// The external resource type
	/// </summary>
	[DataMember(Name = "externalResourceType", IsRequired = false)]
	public string? ExternalResourceType { get; set; }

	/// <summary>
	/// OID
	/// </summary>
	[DataMember(Name = "OID", IsRequired = true)]
	public string Oid { get; set; } = null!;

	/// <summary>
	/// The instance level properties
	/// </summary>
	[DataMember(Name = "ILP", IsRequired = false)]
	public List<InstanceLevelProperty>? InstanceLevelProperties { get; set; }

	/// <summary>
	/// Discovery Type
	/// </summary>
	[DataMember(Name = "discoveryType", IsRequired = true)]
	public string? DiscoveryType { get; set; }
}
