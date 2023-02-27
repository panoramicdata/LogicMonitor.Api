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
	[DataMember(Name = "azureCostManagementType")]
	public string AzureCostManagementType { get; set; } = null!;
}
