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
	public string AlertExpression { get; set; }

	/// <summary>
	///     The Alert expression note
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExpressionNote { get; set; }

	/// <summary>
	///     The Alert transition interval
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object? AlertingDisabledOn { get; set; }

	/// <summary>
	///     The current Alert Id
	/// </summary>
	[DataMember(Name = "currentAlertId")]
	public int CurrentAlertId { get; set; }

	/// <summary>
	///     The dataPoint description
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; }

	/// <summary>
	///     The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	///     The dataSourceInstanceAlias
	/// </summary>
	[DataMember(Name = "dataSourceInstanceAlias")]
	public string DataSourceInstanceAlias { get; set; }

	/// <summary>
	///     The dataSourceInstance Id
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	///     The DeviceGroup Full Path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	///     The DeviceGroup Full Path
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///     Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///     Whether to disable DataPoint Alert HostGroups
	/// </summary>
	[DataMember(Name = "disableDpAlertHostGroups")]
	public object DisableDpAlertHostGroups { get; set; }

	/// <summary>
	///     Whether to enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public bool? EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	///     Whether to enable anomaly alert suppression
	/// </summary>
	///
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public bool? EnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	///     The Global alert expression
	/// </summary>
	[DataMember(Name = "globalAlertExpr")]
	public string GlobalAlertExpr { get; set; }

	/// <summary>
	///     Whether to enable anomaly alert generation globally
	/// </summary>
	///
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public bool? GlobalEnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	///     Whether to enable anomaly alert suppression globally
	/// </summary>
	///
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public bool? GlobalEnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	///     Whether the active discover advanced setting is enabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool IsActiveDiscoveryAdvancedSettingEnabled { get; set; }

	/// <summary>
	///     The parent DeviceGroup Alert Expression List
	/// </summary>
	[DataMember(Name = "parentDeviceGroupAlertExprList")]
	public List<ParentDeviceGroupAlertExpression> ParentDeviceGroupAlertExprList { get; set; }

	/// <summary>
	///     The warn active discovery advanced setting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public bool? WarnActiveDiscvoeryAdvancedSetting { get; set; }

	/// <summary>
	///     The error active discovery advanced setting
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting")]
	public bool? ErrorActiveDiscvoeryAdvancedSetting { get; set; }

	/// <summary>
	///     The critical active discovery advanced setting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public bool? CriticalActiveDiscvoeryAdvancedSetting { get; set; }

	/// <summary>
	///     The collection interval in seconds
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public int CollectionIntervalSeconds { get; set; }

	/// <inheritdoc />
	public override string ToString()
		=> $"{DataPointName} : Expression:{AlertExpression} GlobalExpression: {GlobalAlertExpr}{(DisableAlerting ? " (Disabled)" : string.Empty)}";
}
