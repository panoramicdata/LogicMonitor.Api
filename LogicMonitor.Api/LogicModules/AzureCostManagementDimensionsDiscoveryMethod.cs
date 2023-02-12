using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureCostManagementDimensionsDiscoveryMethod
/// </summary>
[DataContract]
public class AzureCostManagementDimensionsDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// azureCostManagementType
	/// </summary>
	[DataMember(Name = "azureCostManagementType", IsRequired = true)]
	public string AzureCostManagementType { get; set; } = null!;
}
