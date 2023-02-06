using System;
using System.Collections.Generic;
using System.Text;

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
	[DataMember(Name = "period", IsRequired = true)]
	public string Period { get; set; } = null!;

	/// <summary>
	/// metricName
	/// </summary>
	[DataMember(Name = "metricName", IsRequired = false)]
	public string? MetricName { get; set; }

	/// <summary>
	/// dimensionName
	/// </summary>
	[DataMember(Name = "dimensionName", IsRequired = true)]
	public string DimensionName { get; set; } = null!;
}
