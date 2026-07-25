namespace LogicMonitor.Api.Alerts;

/// <summary>
///     Creation DTO for an <see cref="AlertDependencyRule"/> (Dependent Alert Mapping rule).
/// </summary>
[DataContract]
public class AlertDependencyRuleCreationDto : CreationDto<AlertDependencyRule>, IHasName, IHasDescription
{
	/// <summary>
	///     The rule name.
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///     The rule description.
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

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
}
