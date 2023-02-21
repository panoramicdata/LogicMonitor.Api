namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// GcpBillingDiscoveryMethodV3
/// </summary>

[DataContract]
public class GcpBillingDiscoveryMethodV3
{
	/// <summary>
	/// gcpBillingType
	/// </summary>
	[DataMember(Name = "gcpBillingType", IsRequired = true)]
	public string GcpBillingType { get; set; } = null!;
}
