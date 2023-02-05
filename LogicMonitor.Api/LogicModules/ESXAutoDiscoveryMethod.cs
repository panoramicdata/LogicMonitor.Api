using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// ESXAutoDiscoveryMethod
/// </summary>

[DataContract]
public class ESXAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The entity
	/// </summary>
	[DataMember(Name = "entity", IsRequired = true)]
	public string Entity { get; set; } = null!;
}
