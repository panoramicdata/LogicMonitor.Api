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
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	///     The Resource Group Id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///     The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; }

	/// <summary>
	///     Whether alerting is enabled
	/// </summary>
	[DataMember(Name = "alertEnabled")]
	public bool IsAlertingEnabled { get; set; }

	/// <summary>
	///     The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; }

	/// <summary>
	///     Whether Anomaly Alert Generation is enabled
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string IsAnomalyAlertGenerationEnabled { get; set; }

	/// <summary>
	///     Whether anomaly alert suppression is enabled
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string IsAnomalyAlertSuppressionEnabled { get; set; }
}
