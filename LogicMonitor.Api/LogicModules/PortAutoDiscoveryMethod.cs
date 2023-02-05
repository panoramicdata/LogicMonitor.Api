using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// PortAutoDiscoveryMethod
/// </summary>

[DataContract]
public class PortAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The timeout in ms
	/// </summary>
	[DataMember(Name = "timeout", IsRequired = false)]
	public int Timeout { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = true)]
	public string Ports { get; set; } = null!;
}
