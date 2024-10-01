﻿namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource alert SDT
/// </summary>
[DataContract]
public class ResourceAlertSdt : AlertSdt
{
	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "hostId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceId instead", true)]
	public int DeviceId => ResourceId;
}