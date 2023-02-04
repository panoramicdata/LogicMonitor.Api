using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAzureResourceLogAnalyticsWorkspacesSource
/// </summary>

[DataContract]
public class RestAzureResourceLogAnalyticsWorkspacesSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }

	/// <summary>
	/// Azure Log Analytics Workspaces Query
	/// </summary>
	[DataMember(Name = "query", IsRequired = false)]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// Column Instance Name
	/// </summary>
	[DataMember(Name = "columnInstanceName", IsRequired = false)]
	public string ColumnInstanceName { get; set; } = string.Empty;
}
