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
	public string GlobalAlertExpr { get; set; }

	/// <summary>
	/// enableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// criticalAdAdvSetting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticalAdAdvSetting { get; set; }

	/// <summary>
	/// disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// alertExprNote
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
	/// dataPointDescription
	/// </summary>
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; }

	/// <summary>
	/// globalEnableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertGeneration")]
	public string GlobalEnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// enableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// warnAdAdvSetting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; }

	/// <summary>
	/// dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; } 

	/// <summary>
	/// dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// globalEnableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertSuppression")]
	public string GlobalEnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// collectionInterval
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public int CollectionInterval { get; set; }

	/// <summary>
	/// alertExpr
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } 
}
