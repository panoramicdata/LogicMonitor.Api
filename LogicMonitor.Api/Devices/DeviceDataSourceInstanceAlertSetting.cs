namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceInstanceAlertSetting
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceAlertSetting
{
	/// <summary>
	/// The global alert expression for this datapoint
	/// </summary>
	[DataMember(Name = "globalAlertExpr")]
	public string GlobalAlertExpr { get; set; }

	/// <summary>
	/// Instance group alert expression list base on the priority. The first is the highest priority and effected on this instance
	/// </summary>
	[DataMember(Name = "parentInstanceGroupAlertExpr")]
	public InstanceGroupAlertThresholdInfo? ParentInstanceGroupAlertExpr { get; set; }

	/// <summary>
	/// Whether or not alerting will be disabled for the datapoint
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The description of the datapoint the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; }

	/// <summary>
	/// The global enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public string GlobalEnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// The group full path lists who disable alert for this datapoint on devicegroup level
	/// </summary>
	[DataMember(Name = "disableDpAlertHostGroups")]
	public string DisableDpAlertHostGroups { get; set; }

	/// <summary>
	/// The name of the datapoint the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	/// The id of the Datapoint alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The global enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public string GlobalEnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// The ID of the device group
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// The full path of the device group
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	/// The interval of alert transition
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	/// enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// The interval of alert clear transition
	/// </summary>
	[DataMember(Name = "alertClearInterval")]
	public int AlertClearInterval { get; set; }

	/// <summary>
	/// criticalAdAdvSetting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticalAdAdvSetting { get; set; }

	/// <summary>
	/// The note associated with the current alert threshold settings
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExprNote { get; set; }

	/// <summary>
	/// adAdvSettingEnabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool AdAdvSettingEnabled { get; set; }

	/// <summary>
	/// errorAdAdvSetting
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting")]
	public string ErrorAdAdvSetting { get; set; }

	/// <summary>
	/// The id of the DataSource instance alert settings apply to
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	/// warnAdAdvSetting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; }

	/// <summary>
	/// Device group alert expression list base on the priority. The first is the highest priority and effected on this instance
	/// </summary>
	[DataMember(Name = "parentDeviceGroupAlertExprList")]
	public DeviceGroupAlertThresholdInfo? ParentDeviceGroupAlertExprList { get; set; }

	/// <summary>
	/// The datapoint is effected alert disabled by which group
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public string AlertingDisabledOn { get; set; }

	/// <summary>
	/// The alias (name) of the DataSource instance the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataSourceInstanceAlias")]
	public string DataSourceInstanceAlias { get; set; }

	/// <summary>
	/// Collection Interval
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public long CollectionInterval { get; set; }

	/// <summary>
	/// The thresholds that should be associated with the datapoint. Note that you need to have a space between the operator and each threshold (e.g. \u003e 1 2 3)
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; }
}
