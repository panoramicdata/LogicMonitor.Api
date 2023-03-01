namespace LogicMonitor.Api.Collectors;

/// <summary>
/// NetflowAlertModulesCollectorAttributeV3
/// </summary>
[DataContract]
public class NetflowAlertModulesCollectorAttributeV3 : AttributeCollector
{
	/// <summary>
	/// typeOfAlert
	/// </summary>
	[DataMember(Name = "typeOfAlert")]
	public string TypeOfAlert { get; set; } = string.Empty;

	/// <summary>
	/// thresholdValueUnit
	/// </summary>
	[DataMember(Name = "thresholdValueUnit")]
	public string ThresholdValueUnit { get; set; } = string.Empty;

	/// <summary>
	/// netflowFilters
	/// </summary>
	[DataMember(Name = "netflowFilters")]
	public string NetflowFilters { get; set; } = string.Empty;

	/// <summary>
	/// trafficType
	/// </summary>
	[DataMember(Name = "trafficType")]
	public string TrafficType { get; set; } = string.Empty;

	/// <summary>
	/// alertEnable
	/// </summary>
	[DataMember(Name = "alertEnable")]
	public bool AlertEnable { get; set; }

	/// <summary>
	/// topThresholdExpression
	/// </summary>
	[DataMember(Name = "topThresholdExpression")]
	public string TopThresholdExpression { get; set; } = string.Empty;

	/// <summary>
	/// dataDuration
	/// </summary>
	[DataMember(Name = "dataDuration")]
	public string DataDuration { get; set; } = string.Empty;

	/// <summary>
	/// enabled
	/// </summary>
	[DataMember(Name = "enabled")]
	public bool Enabled { get; set; }
}
