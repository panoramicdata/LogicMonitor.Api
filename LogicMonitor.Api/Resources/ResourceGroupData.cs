namespace LogicMonitor.Api.Resources;

/// <summary>
/// ResourceGroup Data
/// </summary>
[DataContract]
public class ResourceGroupData : NamedItem
{
	/// <summary>
	/// The full path of the ResourceGroup (For example, if the group \u0027Dev\u0027 is under a parent group named \u0027Production\u0027, the fullPath would be \u0027Production/Dev\u0027
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string FullPath { get; set; } = string.Empty;

	/// <summary>
	/// The type of ResourceGroup: normal and dynamic ResourceGroups will have groupType\u003dNormal, and AWS groups will have a groupType value of AWS/SERVICE (e.g. AWS/S3)
	/// </summary>
	[DataMember(Name = "groupType")]
	public string GroupType { get; set; } = string.Empty;

	/// <summary>
	/// The permissions for the ResourceGroup that are granted to the user that made this API request
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	/// gcpRegionsInfo
	/// </summary>
	[DataMember(Name = "gcpRegionsInfo")]
	public string GcpRegionsInfo { get; set; } = string.Empty;

	/// <summary>
	/// The Applies to custom query for this group (only for dynamic groups)
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The role privilege operations for the ResourceGroup that are granted to the user that made this API request
	/// </summary>
	[DataMember(Name = "rolePrivileges")]
	public List<string> RolePrivileges { get; set; } = [];

	/// <summary>
	/// The number of instances in each AWS region (only applies to AWS groups)
	/// </summary>
	[DataMember(Name = "awsRegionsInfo")]
	public string AwsRegionsInfo { get; set; } = string.Empty;

	/// <summary>
	/// The number of total Resources, including both AWS and normal devices, that belong to this ResourceGroup (includes normal devices in sub groups)
	/// </summary>
	[DataMember(Name = "numOfHosts")]
	public int ResourceCount { get; set; }

	/// <summary>
	///	Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceCount", true)]
	public int NumOfHosts => ResourceCount;

	/// <summary>
	/// The number of sub-groups that belong only to this ResourceGroup (doesn\u0027t include groups under sub-groups)
	/// </summary>
	[DataMember(Name = "numOfDirectSubGroups")]
	public string NumOfDirectSubGroups { get; set; } = string.Empty;

	/// <summary>
	/// The number of AWS and normal devices that belong only to this ResourceGroup (doesn\u0027t include devices in sub-groups)
	/// </summary>
	[DataMember(Name = "numOfDirectDevices")]
	public string DirectResourceCount { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use DirectResourceCount", true)]
	public string NumOfDirectDevices => DirectResourceCount;

	/// <summary>
	/// The number of instances in each Azure region (only applies to Azure groups)
	/// </summary>
	[DataMember(Name = "azureRegionsInfo")]
	public string AzureRegionsInfo { get; set; } = string.Empty;
}
