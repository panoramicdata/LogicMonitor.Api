using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasSalesforceJsonCollectorAttributeV3
/// </summary>

[DataContract]
public class SaasSalesforceJsonCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }

	/// <summary>
	/// endpointUrlSuffix
	/// </summary>
	[DataMember(Name = "endpointUrlSuffix")]
	public string EndpointUrlSuffix { get; set; }
}
