using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// SyntheticsSeleniumAutoDiscoveryMethodV3
/// </summary>

[DataContract]
public class SyntheticsSeleniumAutoDiscoveryMethodV3 : AutoDiscoveryMethod
{
	/// <summary>
	/// isInternal
	/// </summary>
	[DataMember(Name = "isInternal", IsRequired = false)]
	public bool IsInternal { get; set; }

	/// <summary>
	/// checkpoints
	/// </summary>
	[DataMember(Name = "checkpoints", IsRequired = false)]
	public string? Checkpoints { get; set; }
}
