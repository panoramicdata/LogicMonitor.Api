namespace LogicMonitor.Api.Settings;

/// <summary>
///     Device groups info detail
/// </summary>
[DataContract]
public class DeviceGroupInfoDetail
{
	/// <summary>
	///     The device groups info
	/// </summary>
	[DataMember(Name = "size")]
	public int Count { get; set; }

	/// <summary>
	///     The property count
	/// </summary>
	[DataMember(Name = "props")]
	public int PropertyCount { get; set; }
}

