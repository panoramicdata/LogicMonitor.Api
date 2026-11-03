namespace LogicMonitor.Api.Settings;

/// <summary>
/// Whether an action rule is enabled
/// </summary>
[DataContract]
public class ActionRuleStatus
{
	/// <summary>
	/// Indicates whether the action rule is enabled
	/// </summary>
	[DataMember(Name = "enabled")]
	public bool Enabled { get; set; }
}
