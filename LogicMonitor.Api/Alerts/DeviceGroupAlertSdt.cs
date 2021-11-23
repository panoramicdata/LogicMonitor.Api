using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A device group alert SDT
/// </summary>
[DataContract]
public class DeviceGroupAlertSdt : AlertSdt
{
	/// <summary>
	/// The device group id
	/// </summary>
	[DataMember(Name = "hostGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }
}
