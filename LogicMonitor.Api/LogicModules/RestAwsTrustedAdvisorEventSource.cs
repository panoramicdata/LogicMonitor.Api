using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAwsTrustedAdvisorEventSource
/// </summary>

[DataContract]
public class RestAwsTrustedAdvisorEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }
}
