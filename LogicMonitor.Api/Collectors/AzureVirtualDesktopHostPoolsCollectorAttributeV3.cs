using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// AzureVirtualDesktopHostPoolsCollectorAttributeV3
/// </summary>

[DataContract]
public class AzureVirtualDesktopHostPoolsCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }
}
