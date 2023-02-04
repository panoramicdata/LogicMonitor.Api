using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// The instance group alert threshold info
/// </summary>

[DataContract]
public class DeviceGroupAlertThresholdInfo
{
	/// <summary>
	/// enable anomaly alert generation
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration", IsRequired = false)]
	public string EnableAnomalyAlertGeneration { get; set; }

	/// <summary>
	/// userPermission
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	public string UserPermission { get; set; }

	/// <summary>
	/// enable anomaly alert suppression
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression", IsRequired = false)]
	public string EnableAnomalyAlertSuppression { get; set; }

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId", IsRequired = false)]
	public int GroupId { get; set; }

	/// <summary>
	/// Whether alerts are enabled
	/// </summary>
	[DataMember(Name = "alertEnabled", IsRequired = false)]
	public bool AlertEnabled { get; set; }

	/// <summary>
	/// groupFullPath
	/// </summary>
	[DataMember(Name = "groupFullPath", IsRequired = false)]
	public string GroupFullPath { get; set; }

	/// <summary>
	/// The alert expr
	/// </summary>
	[DataMember(Name = "alertExpr", IsRequired = false)]
	public string AlertExpr { get; set; }
}
