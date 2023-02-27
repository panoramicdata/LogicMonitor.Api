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
	[DataMember(Name = "schedule")]
	public int Schedule { get; set; }

	/// <summary>
	/// Azure Log Analytics Workspaces Query
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// Column Instance Name
	/// </summary>
	[DataMember(Name = "columnInstanceName")]
	public string ColumnInstanceName { get; set; } = string.Empty;
}
