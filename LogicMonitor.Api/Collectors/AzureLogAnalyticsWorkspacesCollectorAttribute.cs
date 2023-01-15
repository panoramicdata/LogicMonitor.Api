using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureLogAnalyticsWorkspacesCollectorAttribute
/// </summary>

[DataContract]
public class AzureLogAnalyticsWorkspacesCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Query
	/// </summary>
	[DataMember(Name = "query")]
	public int Query { get; set; }
}
