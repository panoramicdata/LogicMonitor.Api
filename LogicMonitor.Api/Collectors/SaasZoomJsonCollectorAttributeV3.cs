using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasZoomJsonCollectorAttributeV3
/// </summary>

[DataContract]
public class SaasZoomJsonCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// endpointUrlSuffix
	/// </summary>
	[DataMember(Name = "endpointUrlSuffix")]
	public string EndpointUrlSuffix { get; set; }

	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
