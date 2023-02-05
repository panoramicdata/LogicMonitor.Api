using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
///
/// </summary>

[DataContract]
public class AwsServiceRegionDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The AWS service name
	/// </summary>
	[DataMember(Name = "awsServiceName", IsRequired = true)]
	public string AwsServiceName { get; set; } = null!;
}
