﻿namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource alert SDT
/// </summary>
[DataContract]
public class ResourceGroupAlertSdt : AlertSdt
{
	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "hostGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;

	/// <summary>
	/// The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }
}

