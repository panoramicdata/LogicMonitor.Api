namespace LogicMonitor.Api.Alerts;

/// <summary>
///     A Dependent Alert Mapping (DAM) rule. When resources matched by the rule alert, the rule
///     controls whether reachability / non-reachability alert routing is suppressed (optionally after
///     a delay), so downstream/dependent alerts are not routed while a dependency is down.
///     Endpoint: <c>setting/alert/dependencyrules</c>.
/// </summary>
[DataContract]
public class AlertDependencyRule : NamedItem, IHasEndpoint
{
	/// <summary>
	///     The rule priority (lower numbers are evaluated first).
	/// </summary>
	[DataMember(Name = "priority")]
	public int Priority { get; set; }

	/// <summary>
	///     Whether routing of alerts is delayed before suppression takes effect.
	/// </summary>
	[DataMember(Name = "enableRoutingDelay")]
	public bool EnableRoutingDelay { get; set; }

	/// <summary>
	///     The delay, in minutes, applied when <see cref="EnableRoutingDelay"/> is true.
	/// </summary>
	[DataMember(Name = "delayMinutes")]
	public int DelayMinutes { get; set; }

	/// <summary>
	///     Whether routing of reachability alerts is disabled (suppressed) for matched resources.
	/// </summary>
	[DataMember(Name = "disableReachAlertRouting")]
	public bool DisableReachAlertRouting { get; set; }

	/// <summary>
	///     Whether routing of non-reachability alerts is disabled (suppressed) for matched resources.
	/// </summary>
	[DataMember(Name = "disableNonReachAlertRouting")]
	public bool DisableNonReachAlertRouting { get; set; }

	/// <summary>
	///     The resources (group + resource pattern) the rule applies to.
	/// </summary>
	[DataMember(Name = "resourceMatchPatternList")]
	public List<ResourceMatchPattern> ResourceMatchPatternList { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "setting/alert/dependencyrules";

	/// <inheritdoc />
	public override string ToString() => $"{Id} : {Name}";
}
