using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasZoomPlanUsageCollectorAttributeV3
/// </summary>

[DataContract]
public class SaasZoomPlanUsageCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }

	/// <summary>
	/// Plan type
	/// </summary>
	[DataMember(Name = "planType")]
	public string PlanType { get; set; }
}
