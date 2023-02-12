using System;
using System.Collections.Generic;
using System.Text;

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
	[DataMember(Name = "columnInstanceName", IsRequired = true)]
	public string ColumnInstanceName { get; set; } = null!;
}
