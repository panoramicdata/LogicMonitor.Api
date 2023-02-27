namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureLogAnalyticsWorkspacesDiscoveryMethod
/// </summary>

[DataContract]
public class AzureLogAnalyticsWorkspacesDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// columnInstanceName
	/// </summary>
	[DataMember(Name = "columnInstanceName")]
	public string ColumnInstanceName { get; set; } = null!;
}
