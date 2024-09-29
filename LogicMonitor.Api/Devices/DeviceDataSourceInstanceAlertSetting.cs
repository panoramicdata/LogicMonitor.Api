namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceInstanceAlertSetting
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceAlertSetting
{
	/// <summary>
	/// The global alert expression for this DataPoint
	/// </summary>
	[DataMember(Name = "globalAlertExpr")]
	public string GlobalAlertExpr { get; set; } = string.Empty;

	/// <summary>
	/// Instance group alert expression list base on the priority. The first is the highest priority and effected on this instance
	/// </summary>
	[DataMember(Name = "parentInstanceGroupAlertExpr")]
	public InstanceGroupAlertThresholdInfo ParentInstanceGroupAlertExpr { get; set; } = new();

	/// <summary>
	/// Whether or not alerting will be disabled for the DataPoint
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The description of the DataPoint the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; } = string.Empty;

	/// <summary>
	/// The global enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public string GlobalEnableAnomalyAlertGeneration { get; set; } = string.Empty;  // STRING not a bool

	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;    // STRING not a bool

	/// <summary>
	/// The group full path lists who disable alert for this DataPoint on ResourceGroup level
	/// </summary>
	[DataMember(Name = "disableDpAlertHostGroups")]
	public string DisableDpAlertHostGroups { get; set; } = string.Empty;

	/// <summary>
	/// The name of the DataPoint the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The id of the DataPoint alert settings apply to
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The global enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public string GlobalEnableAnomalyAlertSuppression { get; set; } = string.Empty;     // STRING not a bool e.g. "1,1,1"

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;

	/// <summary>
	/// The full path of the ResourceGroup
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupFullPath", true)]
	public string DeviceGroupFullPath => ResourceGroupFullPath;

	/// <summary>
	/// The interval of alert transition
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	/// enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	/// The interval of alert clear transition
	/// </summary>
	[DataMember(Name = "alertClearInterval")]
	public int AlertClearInterval { get; set; }

	/// <summary>
	/// criticalAdAdvSetting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticalAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// The note associated with the current alert threshold settings
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExprNote { get; set; } = string.Empty;

	/// <summary>
	/// adAdvSettingEnabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool AdAdvSettingEnabled { get; set; }

	/// <summary>
	/// errorAdAdvSetting
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting")]
	public string ErrorAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// The id of the DataSource instance alert settings apply to
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	/// warnAdAdvSetting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// ResourceGroup alert expressions, based on the priority. The first is the highest priority and effected on this instance
	/// </summary>
	[DataMember(Name = "parentDeviceGroupAlertExprList")]
	public ResourceGroupAlertThresholdInfo ParentResourceGroupAlertExpressions { get; set; } = new();

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupName instead", true)]
	public ResourceGroupAlertThresholdInfo ParentDeviceGroupAlertExprList => ParentResourceGroupAlertExpressions;

	/// <summary>
	/// The DataPoint is effected alert disabled by which group
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public string AlertingDisabledOn { get; set; } = string.Empty;

	/// <summary>
	/// The alias (name) of the DataSource instance the alert settings apply to
	/// </summary>
	[DataMember(Name = "dataSourceInstanceAlias")]
	public string DataSourceInstanceAlias { get; set; } = string.Empty;

	/// <summary>
	/// Collection Interval
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public long CollectionInterval { get; set; }

	/// <summary>
	/// The thresholds that should be associated with the datapoint. Note that you need to have a space between the operator and each threshold (e.g. \u003e 1 2 3)
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;
}
