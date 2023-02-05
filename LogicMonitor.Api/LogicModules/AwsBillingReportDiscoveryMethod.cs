using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;
/// <summary>
/// AwsBillingReportDiscoveryMethod
/// </summary>

[DataContract]
public class AwsBillingReportDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The AWS billing reporting attribute
	/// </summary>
	[DataMember(Name = "awsBillingReportAttribute", IsRequired = true)]
	public string AwsBillingReportAttribute { get; set; } = null!;

}
