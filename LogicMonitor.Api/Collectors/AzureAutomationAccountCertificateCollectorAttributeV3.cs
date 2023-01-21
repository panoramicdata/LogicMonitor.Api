using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureAutomationAccountCertificateCollectorAttributeV3
/// </summary>

[DataContract]
public class AzureAutomationAccountCertificateCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
