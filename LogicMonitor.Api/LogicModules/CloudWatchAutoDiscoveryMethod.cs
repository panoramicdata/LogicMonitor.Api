using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// CloudWatchAutoDiscoveryMethod
/// </summary>

[DataContract]
public class CloudWatchAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The cluster dimension
	/// </summary>
	[DataMember(Name = "clusterDimension", IsRequired = true)]
	public string ClusterDimension { get; set; } = null!;

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period", IsRequired = false)]
	public string? Period { get; set; }

	/// <summary>
	/// Metric Name
	/// </summary>
	[DataMember(Name = "metricName", IsRequired = false)]
	public string? MetricName { get; set; }

	/// <summary>
	/// Node dimension
	/// </summary>
	[DataMember(Name = "nodeDimension", IsRequired = true)]
	public string? NodeDimension { get; set; } = null!;

	/// <summary>
	/// namespace
	/// </summary>
	[DataMember(Name = "namespace", IsRequired = true)]
	public string Namespace { get; set; } = null!;

	/// <summary>
	/// The cluster dimension value
	/// </summary>
	[DataMember(Name = "clusterDimensionValue", IsRequired = false)]
	public string? ClusterDimensionValue { get; set; }
}
