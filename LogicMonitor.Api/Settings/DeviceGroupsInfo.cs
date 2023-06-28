namespace LogicMonitor.Api.Settings;

/// <summary>
/// Device Groups Info
/// </summary>
[DataContract]
public class DeviceGroupsInfo
{
	/// <summary>
	///     The device groups info details for static device groups
	/// </summary>
	[DataMember(Name = "static")]
	public DeviceGroupInfoDetail Static { get; set; } = new();

	/// <summary>
	///     The device groups info details for dynamic device groups
	/// </summary>
	[DataMember(Name = "dynamic")]
	public DeviceGroupInfoDetail Dynamic { get; set; } = new();
}