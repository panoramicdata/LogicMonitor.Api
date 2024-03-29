namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A device map point
/// </summary>
[DataContract]
public class DeviceMapPoint : MapPoint
{
	/// <summary>
	///    Constructor
	/// </summary>
	public DeviceMapPoint()
	{
		Type = "device";
	}

	/// <summary>
	///    The deviceGroup Full Path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///    The device DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    Whether it has location
	/// </summary>
	[DataMember(Name = "hasLocation")]
	public bool HasLocation { get; set; }
}
