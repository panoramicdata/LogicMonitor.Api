namespace LogicMonitor.Api.Usage;

/// <summary>
/// A contract subscription
/// </summary>
[DataContract]
public class ContractSubscription
{
	/// <summary>
	/// The feature
	/// </summary>
	[DataMember(Name = "feature")]
	public string Feature { get; set; } = string.Empty;

	/// <summary>
	/// The quantity
	/// </summary>
	[DataMember(Name = "quantity")]
	public double Quantity { get; set; }

	/// <summary>
	/// The retention days
	/// </summary>
	[DataMember(Name = "retentionDays")]
	public int RetentionDays { get; set; }

	/// <summary>
	/// The sku
	/// </summary>
	[DataMember(Name = "sku")]
	public string Sku { get; set; } = string.Empty;
}
