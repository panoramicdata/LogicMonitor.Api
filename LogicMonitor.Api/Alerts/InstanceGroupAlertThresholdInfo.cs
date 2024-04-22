namespace LogicMonitor.Api.Alerts;

/// <summary>
/// The instance group alert threshold info
/// </summary>

[DataContract]
public class InstanceGroupAlertThresholdInfo
{
	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;    // STRING not a bool

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
	/// The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// The alert transition interval
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public string AlertTransitionInterval { get; set; } = string.Empty;

	/// <summary>
	/// The alert clear transition interval
	/// </summary>
	[DataMember(Name = "alertClearTransitionInterval")]
	public string AlertClearTransitionInterval { get; set; } = string.Empty;

	/// <summary>
	/// The alert for no data value
	/// </summary>
	[DataMember(Name = "alertForNoData")]
	public int AlertForNoData { get; set; }

}
