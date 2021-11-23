using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A device data source instance SDT
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceAlertSdt : AlertSdt
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
