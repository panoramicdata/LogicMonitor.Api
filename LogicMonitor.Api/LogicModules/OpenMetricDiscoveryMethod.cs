using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;
/// <summary>
/// OpenMetricDiscoveryMethod
/// </summary>

[DataContract]
public class OpenMetricDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// Headers
	/// </summary>
	[DataMember(Name = "headers", IsRequired = true)]
	public string Headers { get; set; } = null!;

	/// <summary>
	/// Metric name
	/// </summary>
	[DataMember(Name = "metricName", IsRequired = true)]
	public string MetricName { get; set; } = null!;

	/// <summary>
	/// Read Timeout ms
	/// </summary>
	[DataMember(Name = "readTimeout", IsRequired = false)]
	public int ReadTimeoutMs { get; set; }

	/// <summary>
	/// Whether to follow redirect
	/// </summary>
	[DataMember(Name = "followRedirect", IsRequired = true)]
	public bool FollowRedirect { get; set; }

	/// <summary>
	/// Connect Timeout ms
	/// </summary>
	[DataMember(Name = "connectTimeout", IsRequired = false)]
	public int ConnectTimeoutMs { get; set; }

	/// <summary>
	/// Group Label
	/// </summary>
	[DataMember(Name = "groupLabel", IsRequired = false)]
	public string? GroupLabel { get; set; }

	/// <summary>
	/// Instance Label
	/// </summary>
	[DataMember(Name = "instanceLabel", IsRequired = true)]
	public string InstanceLabel { get; set; } = null!;

	/// <summary>
	/// Instance Property Tags
	/// </summary>
	[DataMember(Name = "instancePropertyTags", IsRequired = false)]
	public string? InstancePropertyTags { get; set; }

	/// <summary>
	/// The URL
	/// </summary>
	[DataMember(Name = "url", IsRequired = true)]
	public string Url { get; set; } = null!;
}
