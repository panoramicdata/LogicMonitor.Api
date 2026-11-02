namespace LogicMonitor.Api.Usage;

/// <summary>
/// Contract information for a child or nested product
/// </summary>
[DataContract]
public class ContractInfoProduct
{
	/// <summary>
	/// The aggregation type
	/// </summary>
	[DataMember(Name = "aggregationType")]
	public string AggregationType { get; set; } = string.Empty;

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category")]
	public string Category { get; set; } = string.Empty;

	/// <summary>
	/// The child products
	/// </summary>
	[DataMember(Name = "childProducts")]
	public List<ContractInfoProduct> ChildProducts { get; set; } = [];

	/// <summary>
	/// The feature
	/// </summary>
	[DataMember(Name = "feature")]
	public string Feature { get; set; } = string.Empty;

	/// <summary>
	/// The product
	/// </summary>
	[DataMember(Name = "product")]
	public string Product { get; set; } = string.Empty;

	/// <summary>
	/// The quantity
	/// </summary>
	[DataMember(Name = "quantity")]
	public List<double> Quantity { get; set; } = [];

	/// <summary>
	/// The rounding required
	/// </summary>
	[DataMember(Name = "roundingRequired")]
	public bool RoundingRequired { get; set; }

	/// <summary>
	/// The subscriptions
	/// </summary>
	[DataMember(Name = "subscriptions")]
	public List<ContractSubscription> Subscriptions { get; set; } = [];

	/// <summary>
	/// The unit
	/// </summary>
	[DataMember(Name = "unit")]
	public List<string> Unit { get; set; } = [];

	/// <summary>
	/// The unpaid
	/// </summary>
	[DataMember(Name = "unpaid")]
	public bool Unpaid { get; set; }
}
