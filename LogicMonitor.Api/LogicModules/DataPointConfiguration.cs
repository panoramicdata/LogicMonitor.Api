namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     The DataPoint configuration
/// </summary>
[DataContract]
public class DataPointConfiguration : IdentifiedItem
{
	/// <summary>
	///     The Alert clear interval
	/// </summary>
	[DataMember(Name = "alertClearInterval")]
	public int AlertClearInterval { get; set; }

	/// <summary>
	///     The Alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	///     The Alert expression note
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExpressionNote { get; set; } = string.Empty;

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
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; } = new();

	/// <summary>
	///     The current Alert Id
	/// </summary>
	[DataMember(Name = "currentAlertId")]
	public int CurrentAlertId { get; set; }

	/// <summary>
	///     The dataPoint description
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; } = string.Empty;

	/// <summary>
	///     The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	///     The dataSourceInstanceAlias
	/// </summary>
	[DataMember(Name = "dataSourceInstanceAlias")]
	public string DataSourceInstanceAlias { get; set; } = string.Empty;

	/// <summary>
	///     The dataSourceInstance Id
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	///     Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///     Whether to disable DataPoint Alert ResourceGroups
	/// </summary>
	[DataMember(Name = "disableDpAlertHostGroups")]
	public object DisableDataPointAlertResourceGroups { get; set; } = new();

	/// <summary>
	///     Whether to enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;       // This is a string NOT a bool e.g. "enableAnomalyAlertGeneration": "1,1,1"

	/// <summary>
	///     Whether to enable anomaly alert suppression
	/// </summary>
	///
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	///     Whether global ad adv setting is enabled
	/// </summary>
	[DataMember(Name = "globalAdAdvSettingEnabled")]
	public bool IsGlobalAdAdvSettingEnabled { get; set; }

	/// <summary>
	///     The global warn Ad adv setting
	/// </summary>
	[DataMember(Name = "globalWarnAdAdvSetting")]
	public string GlobalWarnActiveDiscoveryAdvancedSetting { get; set; } = string.Empty;

	/// <summary>
	///     The global error Ad adv setting
	/// </summary>
	[DataMember(Name = "globalErrorAdAdvSetting")]
	public string GlobalErrorActiveDiscoveryAdvancedSetting { get; set; } = string.Empty;

	/// <summary>
	///     The global critical Ad adv setting
	/// </summary>
	[DataMember(Name = "globalCriticalAdAdvSetting")]
	public string GlobalCriticalActiveDiscoveryAdvancedSetting { get; set; } = string.Empty;

	/// <summary>
	///     The Global alert expression
	/// </summary>
	[DataMember(Name = "globalAlertExpr")]
	public string GlobalAlertExpr { get; set; } = string.Empty;

	/// <summary>
	///     The Global alert transition interval
	/// </summary>
	[DataMember(Name = "globalAlertTransitionInterval")]
	public int GlobalAlertTransitionInterval { get; set; }

	/// <summary>
	///     The Global alert clear transition interval
	/// </summary>
	[DataMember(Name = "globalAlertClearTransitionInterval")]
	public int GlobalAlertClearTransitionInterval { get; set; }

	/// <summary>
	///     The Global alert for no data
	/// </summary>
	[DataMember(Name = "globalAlertForNoData")]
	public int GlobalAlertForNoData { get; set; }

	/// <summary>
	///     Whether to enable anomaly alert generation globally
	/// </summary>
	///
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public string GlobalEnableAnomalyAlertGeneration { get; set; } = string.Empty;       // This is a string NOT a bool e.g. "enableAnomalyAlertGeneration": "1,1,1"

	/// <summary>
	///     Whether to enable anomaly alert suppression globally
	/// </summary>
	///
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public string GlobalEnableAnomalyAlertSuppression { get; set; } = string.Empty;      // STRING not a bool e.g. "1,1,1"

	/// <summary>
	///     Whether the active discover advanced setting is enabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool? IsActiveDiscoveryAdvancedSettingEnabled { get; set; }

	/// <summary>
	///     The parent ResourceGroup Alert Expression List
	/// </summary>
	[DataMember(Name = "parentDeviceGroupAlertExprList")]
	public List<ParentResourceGroupAlertExpression> ParentResourceGroupAlertExprList { get; set; } = [];

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
	///     The collection interval in seconds
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public int CollectionIntervalSeconds { get; set; }

	/// <summary>
	///     The post processor parameter
	/// </summary>
	[DataMember(Name = "postProcessorParam")]
	public string PostProcessorParameter { get; set; } = string.Empty;

	/// <summary>
	///     The global post processor parameter
	/// </summary>
	[DataMember(Name = "globalPostProcessorParam")]
	public string GlobalPostProcessorParameter { get; set; } = string.Empty;

	/// <summary>
	///     Parent Instance Group Alert Expression
	/// </summary>
	[DataMember(Name = "parentInstanceGroupAlertExpr")]
	public InstanceGroupAlertThresholdInfo? ParentInstanceGroupAlertExpression { get; set; }

	/// <summary>
	///     Parent Resource Group Alert Expression
	/// </summary>
	[DataMember(Name = "parentResourceDataSourceAlertExpr")]
	public object? ParentResourceDataSourceAlertExpr { get; set; }

	/// <summary>
	///     Alert for no data
	/// </summary>
	[DataMember(Name = "alertForNoData")]
	public int AlertForNoData { get; set; }

	/// <inheritdoc />
	public override string ToString()
		=> $"{DataPointName} : Expression:{AlertExpression} GlobalExpression: {GlobalAlertExpr}{(DisableAlerting ? " (Disabled)" : string.Empty)}";
}
