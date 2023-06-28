namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     The DataPoint configuration
/// </summary>
[DataContract]
public class DataPointConfigurationCreationDTO : IdentifiedItem
{
	/// <summary>
	/// The thresholds that should be associated with the datapoint. Note that you need to have a space between the operator and each threshold (e.g. > 1 2 3)
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// The note associated with the current alert threshold settings
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExpressionNote { get; set; } = string.Empty;

	/// <summary>
	///     Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///     Whether the active discover advanced setting is enabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool? IsActiveDiscoveryAdvancedSettingEnabled { get; set; }

	/// <summary>
	///     The warn active discovery advanced setting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnActiveDiscoveryAdvancedSetting { get; set; } = string.Empty; // NOTE: do NOT change to bools. In LM they are like this: "1,100,1.5,1,0,5,,"

	/// <summary>
	///     The error active discovery advanced setting
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting")]
	public string ErrorActiveDiscoveryAdvancedSetting { get; set; } = string.Empty; // NOTE: do NOT change to bools. In LM they are like this: "1,100,1.5,1,0,5,,"

	/// <summary>
	///     The critical active discovery advanced setting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticalActiveDiscoveryAdvancedSetting { get; set; } = string.Empty; // NOTE: do NOT change to bools. In LM they are like this: "1,100,1.5,1,0,5,,"

	/// <summary>
	///     Parent Instance Group Alert Expression
	/// </summary>
	[DataMember(Name = "parentInstanceGroupAlertExpr")]
	public string ParentInstanceGroupAlertExpression { get; set; } = string.Empty;
}
