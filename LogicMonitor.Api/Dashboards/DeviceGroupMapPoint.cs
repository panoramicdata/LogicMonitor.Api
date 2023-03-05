namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A group map point
/// </summary>
[DataContract]
public class DeviceGroupMapPoint : MapPoint
{
	/// <summary>
	///    Constructor
	/// </summary>
	public DeviceGroupMapPoint()
	{
		Type = "group";
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
