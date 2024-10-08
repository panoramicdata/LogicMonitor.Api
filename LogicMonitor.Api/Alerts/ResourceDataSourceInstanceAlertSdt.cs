﻿namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource DataSource instance SDT
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceAlertSdt : AlertSdt
{
	/// <summary>
	/// The DataSourceInstance ID (note: these two are identical but both present)
	/// </summary>
	[DataMember(Name = "dsiId")]
	public int DsiId { get; set; }

	/// <summary>
	/// The DataSourceInstance ID (note: these two are identical but both present)
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int DataSourceInstanceId { get; set; }
}
