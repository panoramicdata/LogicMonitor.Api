namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureSynapseDiscoveryMethodV3
/// </summary>

[DataContract]
public class AzureSynapseDiscoveryMethodV3 : AutoDiscoveryMethod
{
	/// <summary>
	/// azureSynapseType
	/// </summary>
	[DataMember(Name = "azureSynapseType", IsRequired = true)]
	public string AzureSynapseType { get; set; } = null!;
}
