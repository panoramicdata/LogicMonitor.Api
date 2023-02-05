using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// GcpBillingBigQuerySourceDiscoveryMethodV3
/// </summary>

[DataContract]
public class GcpBillingBigQuerySourceDiscoveryMethodV3 : AutoDiscoveryMethod
{
	/// <summary>
	/// GCP Billing Period
	/// </summary>
	[DataMember(Name = "gcpBillingPeriodType", IsRequired = true)]
	public string GcpBillingPeriod { get; set; } = null!;

	/// <summary>
	/// GCP Billing Type
	/// </summary>
	[DataMember(Name = "gcpBillingType", IsRequired = true)]
	public string GcpBillingType { get; set; } = null!;
}
