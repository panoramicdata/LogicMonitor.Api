namespace LogicMonitor.Api.Devices;

/// <summary>
/// The ResourceGroup alert threshold info
/// </summary>
[DataContract]
public class ResourceGroupAlertThresholdInfo
{
	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;    // STRING not a bool

	/// <summary>
	/// userPermission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	/// enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// Whether alerts are enabled
	/// </summary>
	[DataMember(Name = "alertEnabled")]
	public bool AlertEnabled { get; set; }

	/// <summary>
	/// groupFullPath
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;
}
