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
	[DataMember(Name = "inAlerting")]
	public bool InAlerting { get; set; }

	/// <summary>
	/// throttlingAlerts
	/// </summary>
	[DataMember(Name = "throttlingAlerts")]
	public int ThrottlingAlerts { get; set; }

	/// <summary>
	/// enableThrottling
	/// </summary>
	[DataMember(Name = "enableThrottling")]
	public bool EnableThrottling { get; set; }

	/// <summary>
	/// destinations
	/// </summary>
	[DataMember(Name = "destinations")]
	public List<Chain> Destinations { get; set; } 

	/// <summary>
	/// ccDestinations
	/// </summary>
	[DataMember(Name = "ccDestinations")]
	public List<Recipient>? CcDestinations { get; set; }

	/// <summary>
	/// throttlingPeriod
	/// </summary>
	[DataMember(Name = "throttlingPeriod")]
	public int ThrottlingPeriod { get; set; }
}
