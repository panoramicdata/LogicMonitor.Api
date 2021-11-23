using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The DataSource Device
/// </summary>
[DataContract]
public class DataSourceDevice
{
	/// <summary>
	/// The deviceDataSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }
}
