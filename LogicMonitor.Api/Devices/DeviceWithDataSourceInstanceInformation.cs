namespace LogicMonitor.Api.Devices;

/// <summary>
/// Device with DataSource instance information
/// </summary>
[DataContract]
public class DeviceWithDataSourceInstanceInformation : Device
{
	/// <summary>
	/// The DataSourceInstance information
	/// </summary>
	[DataMember(Name = "instance")]
	public List<DeviceDataSourceInstanceSummary> DeviceDataSourceInstances { get; set; } = [];
}
