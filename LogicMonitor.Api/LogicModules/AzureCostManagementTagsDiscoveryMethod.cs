namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureCostManagementTagsDiscoveryMethod
/// </summary>

[DataContract]
public class AzureCostManagementTagsDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// azureTagKeys
	/// </summary>
	[DataMember(Name = "azureTagKeys", IsRequired = true)]
	public string AzureTagKeys { get; set; } = null!;
}
