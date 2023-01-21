using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureVMBackupStatusLogAnalyticsCollectorAttribute
/// </summary>

[DataContract]
public class AzureVMBackupStatusLogAnalyticsCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
