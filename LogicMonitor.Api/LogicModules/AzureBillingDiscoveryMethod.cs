using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureBillingDiscoveryMethod
/// </summary>

[DataContract]
public class AzureBillingDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// Azure Billing Type
	/// </summary>
	[DataMember(Name = "azureBillingType", IsRequired = true)]
	public string AzureBillingType { get; set; } = null!;
}
