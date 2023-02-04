using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// NetflowAlertModulesCollectorAttributeV3
/// </summary>
[DataContract]
public class NetflowAlertModulesCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// typeOfAlert
	/// </summary>
	[DataMember(Name = "typeOfAlert", IsRequired = false)]
	public string TypeOfAlert { get; set; } = string.Empty;

	/// <summary>
	/// thresholdValueUnit
	/// </summary>
	[DataMember(Name = "thresholdValueUnit", IsRequired = false)]
	public string ThresholdValueUnit { get; set; } = string.Empty;

	/// <summary>
	/// netflowFilters
	/// </summary>
	[DataMember(Name = "netflowFilters", IsRequired = false)]
	public string NetflowFilters { get; set; } = string.Empty;

	/// <summary>
	/// trafficType
	/// </summary>
	[DataMember(Name = "trafficType", IsRequired = false)]
	public string TrafficType { get; set; } = string.Empty;

	/// <summary>
	/// alertEnable
	/// </summary>
	[DataMember(Name = "alertEnable", IsRequired = false)]
	public bool AlertEnable { get; set; }

	/// <summary>
	/// topThresholdExpression
	/// </summary>
	[DataMember(Name = "topThresholdExpression", IsRequired = false)]
	public string TopThresholdExpression { get; set; } = string.Empty;

	/// <summary>
	/// dataDuration
	/// </summary>
	[DataMember(Name = "dataDuration", IsRequired = false)]
	public string DataDuration { get; set; } = string.Empty;

	/// <summary>
	/// enabled
	/// </summary>
	[DataMember(Name = "enabled", IsRequired = false)]
	public bool Enabled { get; set; }
}
