using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasWebexCollectorAttribute
/// </summary>

[DataContract]
public class SaasWebexCollectorAttribute : CollectorAttribute
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

	/// <summary>
	/// endpointUrlPrefix
	/// </summary>
	[DataMember(Name = "endpointUrlPrefix")]
	public string EndpointUrlPrefix { get; set; }

	/// <summary>
	/// instanceColumnName
	/// </summary>
	[DataMember(Name = "instanceColumnName")]
	public string InstanceColumnName { get; set; }
}
