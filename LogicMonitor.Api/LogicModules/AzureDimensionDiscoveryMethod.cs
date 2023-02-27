namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureDimensionDiscoveryMethod
/// </summary>

[DataContract]
public class AzureDimensionDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// dimensionName
	/// </summary>
	[DataMember(Name = "dimensionName")]
	public string DimensionName { get; set; } = string.Empty;
}
