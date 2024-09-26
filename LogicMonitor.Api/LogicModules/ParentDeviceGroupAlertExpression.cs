namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A ParentDeviceGroupAlertExpression
/// </summary>
[DataContract]
public class ParentDeviceGroupAlertExpression
{
	/// <summary>
	///     The ResourceGroup Full Path
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string DeviceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The Resource Group Id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int DeviceGroupId { get; set; }

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
}
