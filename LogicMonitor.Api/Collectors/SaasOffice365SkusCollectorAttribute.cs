using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasOffice365SkusCollectorAttribute
/// </summary>

[DataContract]
public class SaasOffice365SkusCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }
}
