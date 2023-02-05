using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// PDHAutoDiscoveryMethod
/// </summary>

[DataContract]
public class PDHAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The object regex
	/// </summary>
	[DataMember(Name = "objRegex", IsRequired = true)]
	public string ObjectRegex { get; set; } = null!;

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category", IsRequired = true)]
	public string Category { get; set; } = null!;
}
