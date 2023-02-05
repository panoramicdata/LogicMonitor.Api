using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// SaaSZoomPlanUsageDiscoveryMethod
/// </summary>

[DataContract]
public class SaaSZoomPlanUsageDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// Zoom plan usage type
	/// </summary>
	[DataMember(Name = "zoomPlanUsageType", IsRequired = true)]
	public ZoomPlanUsageType ZoomPlanUsageType { get; set; }
}
