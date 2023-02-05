using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// IPMIAutoDiscoveryMethod
/// </summary>

[DataContract]
public class IPMIAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method", IsRequired = false)]
	public string? Method { get; set; }
}
