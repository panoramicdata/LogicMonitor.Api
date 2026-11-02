namespace LogicMonitor.Api.Usage;

/// <summary>
/// A cloud cost optimization recommendation category
/// </summary>
[DataContract]
public class CloudRecommendationCategory
{
	/// <summary>
	/// Description of the recommendation category
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Name of the recommendation category
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
