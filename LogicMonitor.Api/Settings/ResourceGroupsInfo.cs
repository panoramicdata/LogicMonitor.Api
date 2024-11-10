namespace LogicMonitor.Api.Settings;

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