using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// EscalatingChain
/// </summary>

[DataContract]
public class EscalatingChain : NamedStringIdentifiedItem
{
	/// <summary>
	/// inAlerting
	/// </summary>
	[DataMember(Name = "inAlerting", IsRequired = false)]
	public bool InAlerting { get; set; }

	/// <summary>
	/// throttlingAlerts
	/// </summary>
	[DataMember(Name = "throttlingAlerts", IsRequired = false)]
	public int ThrottlingAlerts { get; set; }

	/// <summary>
	/// enableThrottling
	/// </summary>
	[DataMember(Name = "enableThrottling", IsRequired = false)]
	public bool EnableThrottling { get; set; }

	/// <summary>
	/// destinations
	/// </summary>
	[DataMember(Name = "destinations", IsRequired = true)]
	public List<Chain> Destinations { get; set; } = null!;

	/// <summary>
	/// ccDestinations
	/// </summary>
	[DataMember(Name = "ccDestinations", IsRequired = false)]
	public List<Recipient>? CcDestinations { get; set; }

	/// <summary>
	/// throttlingPeriod
	/// </summary>
	[DataMember(Name = "throttlingPeriod", IsRequired = false)]
	public int ThrottlingPeriod { get; set; }
}
