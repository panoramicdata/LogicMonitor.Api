namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A Parent ResourceGroup Alert Expression
/// </summary>
[DataContract]
public class ParentResourceGroupAlertExpression
{
	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The ResourceGroup full path (alternative API field name)
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	///     The Alert transition interval
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	///     The Alert clear transition interval
	/// </summary>
	[DataMember(Name = "alertClearTransitionInterval")]
	public int AlertClearTransitionInterval { get; set; }

	/// <summary>
	///     Whether to alert for no data
	/// </summary>
	[DataMember(Name = "alertForNoData")]
	public int AlertForNoData { get; set; }

	/// <summary>
	///     The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	///     Whether alerting is enabled
	/// </summary>
	[DataMember(Name = "alertEnabled")]
	public bool IsAlertingEnabled { get; set; }

	/// <summary>
	///     The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	///     Whether Anomaly Alert Generation is enabled
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string IsAnomalyAlertGenerationEnabled { get; set; } = string.Empty;       // This is a string NOT a bool e.g. "enableAnomalyAlertGeneration": "1,1,1"

	/// <summary>
	///     Whether anomaly alert suppression is enabled
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string IsAnomalyAlertSuppressionEnabled { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	///     The dynamic parent group id
	/// </summary>
	[DataMember(Name = "dynamicParentGroupId")]
	public int DynamicParentGroupId { get; set; }

	/// <summary>
	///     Whether AD advanced setting is enabled
	/// </summary>
	[DataMember(Name = "_adAdvSettingEnabled")]
	public bool AdAdvSettingEnabled { get; set; }

	/// <summary>
	///     The warn AD advanced setting
	/// </summary>
	[DataMember(Name = "_warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	///     The error AD advanced setting
	/// </summary>
	[DataMember(Name = "_errorAdAdvSetting")]
	public string ErrorAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	///     The critical AD advanced setting
	/// </summary>
	[DataMember(Name = "_criticalAdAdvSetting")]
	public string CriticalAdAdvSetting { get; set; } = string.Empty;
}
