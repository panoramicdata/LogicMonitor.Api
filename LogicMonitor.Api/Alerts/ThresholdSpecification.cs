namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Threshold specification
/// </summary>
[DataContract]
public class ThresholdSpecification
{
	/// <summary>
	/// The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// The alert expression note
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExpressionNote { get; set; } = string.Empty;

	/// <summary>
	/// Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public string DisableAlerting { get; set; } = string.Empty;
}
