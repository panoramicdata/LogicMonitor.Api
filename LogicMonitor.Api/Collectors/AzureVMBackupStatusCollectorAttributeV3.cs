using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureVMBackupStatusCollectorAttributeV3
/// </summary>

[DataContract]
public class AzureVMBackupStatusCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }
}
