namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureDimensionDiscoveryMethod
/// </summary>

[DataContract]
public class AzureDimensionDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// period
	/// </summary>
	[DataMember(Name = "period")]
	public string Period { get; set; } = null!;

	/// <summary>
	/// metricName
	/// </summary>
	[DataMember(Name = "metricName")]
	public string? MetricName { get; set; }

	/// <summary>
	/// dimensionName
	/// </summary>
	[DataMember(Name = "dimensionName")]
	public string DimensionName { get; set; } = null!;
}
