using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceGroupDataSourceDataPointConfig
/// </summary>
public class DeviceGroupDataSourceDataPointConfig
{
	/// <summary>
	/// globalAlertExpr
	/// </summary>
	[DataMember(Name = "globalAlertExpr", IsRequired = false)]
	public string GlobalAlertExpr { get; set; }

	/// <summary>
	/// enableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression", IsRequired = false)]
	public string EnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// criticalAdAdvSetting
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting", IsRequired = false)]
	public string CriticalAdAdvSetting { get; set; }

	/// <summary>
	/// disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting", IsRequired = false)]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// alertExprNote
	/// </summary>
	[DataMember(Name = "alertExprNote", IsRequired = false)]
	public string AlertExprNote { get; set; }

	/// <summary>
	/// adAdvSettingEnabled
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled", IsRequired = false)]
	public bool AdAdvSettingEnabled { get; set; }

	/// <summary>
	/// errorAdAdvSetting
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting", IsRequired = false)]
	public string ErrorAdAdvSetting { get; set; }

	/// <summary>
	/// dataPointDescription
	/// </summary>
	[DataMember(Name = "dataPointDescription", IsRequired = false)]
	public string DataPointDescription { get; set; }

	/// <summary>
	/// globalEnableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertGeneration", IsRequired = false)]
	public string GlobalEnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// enableAnomalyAlertGeneration
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration", IsRequired = false)]
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// warnAdAdvSetting
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting", IsRequired = false)]
	public string WarnAdAdvSetting { get; set; }

	/// <summary>
	/// dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName", IsRequired = true)]
	public string DataPointName { get; set; } = null!;

	/// <summary>
	/// dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId", IsRequired = true)]
	public int DataPointId { get; set; }

	/// <summary>
	/// globalEnableAnomalyAlertSuppression
	/// </summary>
	[DataMember(Name = "globalEnableAnomalyAlertSuppression", IsRequired = false)]
	public string GlobalEnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// collectionInterval
	/// </summary>
	[DataMember(Name = "collectionInterval", IsRequired = false)]
	public int CollectionInterval { get; set; }

	/// <summary>
	/// alertExpr
	/// </summary>
	[DataMember(Name = "alertExpr", IsRequired = true)]
	public string AlertExpr { get; set; } = null!;
}
