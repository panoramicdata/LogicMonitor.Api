using System.Runtime.Serialization;

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
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	///    The device DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	///    Whether it has location
	/// </summary>
	[DataMember(Name = "hasLocation")]
	public bool HasLocation { get; set; }
}

/*
		{
			"type":"group",
			"deviceGroupFullPath":"##defaultDeviceGroup##",
			"deviceDisplayName":"",
			"hasLocation":true
		}
	 */
