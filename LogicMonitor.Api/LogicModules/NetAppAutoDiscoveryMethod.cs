using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// NetAppAutoDiscoveryMethod
/// </summary>

[DataContract]
public class NetAppAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The request
	/// </summary>
	[DataMember(Name = "request", IsRequired = false)]
	public string? Request { get; set; }

	/// <summary>
	/// The instance name
	/// </summary>
	[DataMember(Name = "instanceName", IsRequired = false)]
	public string? InstanceName { get; set; }

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The instance group name
	/// </summary>
	[DataMember(Name = "instanceGroupName", IsRequired = false)]
	public string? InstanceGroupName { get; set; }

	/// <summary>
	/// The instance value
	/// </summary>
	[DataMember(Name = "instanceValue", IsRequired = false)]
	public string? InstanceValue { get; set; }

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "instanceDescription", IsRequired = false)]
	public string? InstanceDescription { get; set; }

	/// <summary>
	/// The object
	/// </summary>
	[DataMember(Name = "object", IsRequired = false)]
	public string? Object { get; set; }

	/// <summary>
	/// The instance locator
	/// </summary>
	[DataMember(Name = "instanceLocator", IsRequired = false)]
	public string? InstanceLocator { get; set; }
}
