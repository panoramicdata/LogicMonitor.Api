using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// JMXAutoDiscoveryMethod
/// </summary>

[DataContract]
public class JMXAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path", IsRequired = true)]
	public string Path { get; set; } = null!;

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = true)]
	public string Ports { get; set; } = null!;

	/// <summary>
	/// The url
	/// </summary>
	[DataMember(Name = "url", IsRequired = true)]
	public string Url { get; set; } = null!;
}
