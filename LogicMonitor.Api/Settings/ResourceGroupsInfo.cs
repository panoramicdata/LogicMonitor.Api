namespace LogicMonitor.Api.Settings;

/// <summary>
/// DeviceGroups Info
/// </summary>
[Obsolete("Use ResourceGroupsInfo instead", true)]
public class DeviceGroupsInfo : ResourceGroupsInfo
{
}

/// <summary>
/// ResourceGroups Info
/// </summary>
[DataContract]
public class ResourceGroupsInfo
{
	/// <summary>
	///     The resource groups info details for static ResourceGroups
	/// </summary>
	[DataMember(Name = "static")]
	public ResourceGroupsInfoDetail Static { get; set; } = new();

	/// <summary>
	///     The ResourceGroups info details for static ResourceGroups
	/// </summary>
	[DataMember(Name = "dynamic")]
	public ResourceGroupsInfoDetail Dynamic { get; set; } = new();
}