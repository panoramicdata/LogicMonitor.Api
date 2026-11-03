namespace LogicMonitor.Api.Usage;

/// <summary>
/// A cloud cost optimization recommendation
/// </summary>
[DataContract]
public class CloudRecommendation : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// Potential annual cost savings in USD
	/// </summary>
	[DataMember(Name = "annualSavings")]
	public double AnnualSavings { get; set; }

	/// <summary>
	/// ID of the cloud account associated with the recommendation
	/// </summary>
	[DataMember(Name = "cloudAccountId")]
	public string CloudAccountId { get; set; } = string.Empty;

	/// <summary>
	/// Cloud provider type associated with the recommendation
	/// </summary>
	[DataMember(Name = "cloudProvider")]
	public string CloudProvider { get; set; } = string.Empty;

	/// <summary>
	/// Cloud service type associated with the recommendation
	/// </summary>
	[DataMember(Name = "cloudServiceType")]
	public string CloudServiceType { get; set; } = string.Empty;

	/// <summary>
	/// Date when the recommendation was generated, in epoch milliseconds
	/// </summary>
	[DataMember(Name = "createdAtMS")]
	public long CreatedAtMS { get; set; }

	/// <summary>
	/// Criteria for the recommendation
	/// </summary>
	[DataMember(Name = "criteria")]
	public string Criteria { get; set; } = string.Empty;

	/// <summary>
	/// Device subtype associated with the recommendation
	/// </summary>
	[DataMember(Name = "deviceSubtype")]
	public string DeviceSubtype { get; set; } = string.Empty;

	/// <summary>
	/// URL to the resource in the cloud provider's console
	/// </summary>
	[DataMember(Name = "providerConsoleUrl")]
	public string ProviderConsoleUrl { get; set; } = string.Empty;

	/// <summary>
	/// Recommendation details
	/// </summary>
	[DataMember(Name = "recommendation")]
	public string Recommendation { get; set; } = string.Empty;

	/// <summary>
	/// Recommendation category
	/// </summary>
	[DataMember(Name = "recommendationCategory")]
	public string RecommendationCategory { get; set; } = string.Empty;

	/// <summary>
	/// Unique numerical identifier for the recommendation
	/// </summary>
	[DataMember(Name = "recommendationId")]
	public long RecommendationId { get; set; }

	/// <summary>
	/// Status of the recommendation
	/// </summary>
	[DataMember(Name = "recommendationStatus")]
	public string RecommendationStatus { get; set; } = string.Empty;

	/// <summary>
	/// Display name of the resource associated with the recommendation
	/// </summary>
	[DataMember(Name = "resourceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// ID of the resource associated with the recommendation
	/// </summary>
	[DataMember(Name = "resourceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Date when the recommendation was last updated, in epoch milliseconds
	/// </summary>
	[DataMember(Name = "updatedAtMS")]
	public long UpdatedAtMS { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "cost-optimization/recommendations";
}
