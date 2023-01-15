using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureRecoveryServicesVaultAgentsCollectorAttribute
/// </summary>

[DataContract]
public class AzureRecoveryServicesVaultAgentsCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }
}
