using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// CollectorAutoDiscoveryMethod
/// </summary>

[DataContract]
public class CollectorAutoDiscoveryMethod
{
	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId", IsRequired = true)]
	public string CollectorId { get; set; } = null!;
}
