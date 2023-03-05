namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceGroupDataSourceDataPointConfig
/// </summary>
public class DeviceGroupDataSourceDataPointConfig
{
	/// <summary>
	/// globalAlertExpr
	/// </summary>
	[DataMember(Name = "globalAlertExpr")]
	public string GlobalAlertExpr { get; set; } = string.Empty;

	/// <summary>
	/// enableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; } = string.Empty;

	/// <summary>
	/// criticalAdAdvSetting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticalAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// alertExprNote
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
	/// dataPointDescription
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; } = string.Empty;

	/// <summary>
	/// globalEnableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public string GlobalEnableAnomalyAlertGeneration { get; set; } = string.Empty;

	/// <summary>
	/// enableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;

	/// <summary>
	/// warnAdAdvSetting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// globalEnableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public string GlobalEnableAnomalyAlertSuppression { get; set; } = string.Empty;

	/// <summary>
	/// collectionInterval
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public int CollectionInterval { get; set; }

	/// <summary>
	/// alertExpr
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;
}
