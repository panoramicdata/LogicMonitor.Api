using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AwsRdsPerformanceInsightsCollectorAttribute
/// </summary>

[DataContract]
public class AwsRdsPerformanceInsightsCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
