namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureEABillingDiscoveryMethodV3
/// </summary>

[DataContract]
public class AzureEABillingDiscoveryMethodV3 : AutoDiscoveryMethod
{
	/// <summary>
	/// azureEABillingType
	/// </summary>
	[DataMember(Name = "azureEABillingType", IsRequired = true)]
	public string AzureEABillingType { get; set; } = null!;
}
