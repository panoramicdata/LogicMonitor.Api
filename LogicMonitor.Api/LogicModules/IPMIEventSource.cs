using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// IPMIEventSource
/// </summary>

[DataContract]
public class IPMIEventSource : EventSource
{
	/// <summary>
	/// impmi check interval
	/// </summary>
	[DataMember(Name = "checkInterval", IsRequired = true)]
	public int CheckInterval { get; set; }
}
