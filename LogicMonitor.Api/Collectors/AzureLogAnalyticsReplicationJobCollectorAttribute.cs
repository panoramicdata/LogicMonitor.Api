using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureLogAnalyticsReplicationJobCollectorAttribute
/// </summary>

[DataContract]
public class AzureLogAnalyticsReplicationJobCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }
}
