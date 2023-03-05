namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// CollectorAutoDiscoveryMethod
/// </summary>

[DataContract]
public class CollectorAutoDiscoveryMethod
{
	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public string CollectorId { get; set; } = string.Empty;
}
