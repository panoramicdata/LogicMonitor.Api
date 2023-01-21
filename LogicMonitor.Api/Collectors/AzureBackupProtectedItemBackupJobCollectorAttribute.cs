using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureBackupProtectedItemBackupJobCollectorAttribute
/// </summary>

[DataContract]
public class AzureBackupProtectedItemBackupJobCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
