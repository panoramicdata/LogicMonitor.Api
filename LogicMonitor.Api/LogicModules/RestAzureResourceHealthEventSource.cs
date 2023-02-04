using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAzureResourceHealthEventSource
/// </summary>

[DataContract]
public class RestAzureResourceHealthEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }
}
