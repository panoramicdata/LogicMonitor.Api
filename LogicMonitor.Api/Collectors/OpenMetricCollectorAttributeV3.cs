using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// OpenMetricCollectorAttributeV3
/// </summary>

[DataContract]
public class OpenMetricCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// headers
	/// </summary>
	[DataMember(Name = "headers")]
	public string Headers { get; set; }

	/// <summary>
	/// followRedirect
	/// </summary>
	[DataMember(Name = "followRedirect")]
	public string FollowRedirect { get; set; }

	/// <summary>
	/// readTimeout
	/// </summary>
	[DataMember(Name = "readTimeout")]
	public int ReadTimeout { get; set; }

	/// <summary>
	/// connectTimeout
	/// </summary>
	[DataMember(Name = "connectTimeout")]
	public int ConnectTimeout { get; set; }

	/// <summary>
	/// url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }
}
