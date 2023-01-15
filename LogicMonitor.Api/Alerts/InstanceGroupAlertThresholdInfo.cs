using System;
using System.Collections.Generic;
using System.Text;

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
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; }

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
	/// The alert expr
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpr { get; set; }
}
