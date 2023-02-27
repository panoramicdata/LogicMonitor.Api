﻿namespace LogicMonitor.Api.Devices;

/// <summary>
/// The instance group alert threshold info
/// </summary>

[DataContract]
public class DeviceGroupAlertThresholdInfo
{
	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// userPermission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; }

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
	/// groupFullPath
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; }

	/// <summary>
	/// The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; }
}
