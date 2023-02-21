namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceGroupData
/// </summary>

[DataContract]
public class DeviceGroupData : NamedItem
{
	/// <summary>
	/// The full path of the device group (i.e. if the group \u0027Dev\u0027 is under a parent group named \u0027Production\u0027, the fullPath would be \u0027Production/Dev\u0027
	/// </summary>
	[DataMember(Name = "fullPath", IsRequired = false)]
	public string FullPath { get; set; }

	/// <summary>
	/// The type of device group: normal and dynamic device groups will have groupType\u003dNormal, and AWS groups will have a groupType value of AWS/SERVICE (e.g. AWS/S3)
	/// </summary>
	[DataMember(Name = "groupType", IsRequired = false)]
	public string GroupType { get; set; }

	/// <summary>
	/// The permissions for the device group that are granted to the user that made this API request
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	public string UserPermission { get; set; }

	/// <summary>
	/// gcpRegionsInfo
	/// </summary>
	[DataMember(Name = "gcpRegionsInfo", IsRequired = false)]
	public string GcpRegionsInfo { get; set; }

	/// <summary>
	/// The Applies to custom query for this group (only for dynamic groups)
	/// </summary>
	[DataMember(Name = "appliesTo", IsRequired = false)]
	public string AppliesTo { get; set; }

	/// <summary>
	/// The role privilege operations for the device group that are granted to the user that made this API request
	/// </summary>
	[DataMember(Name = "rolePrivileges", IsRequired = false)]
	public List<string>? RolePrivileges { get; set; }

	/// <summary>
	/// The number of instances in each AWS region (only applies to AWS groups)
	/// </summary>
	[DataMember(Name = "awsRegionsInfo", IsRequired = false)]
	public string AwsRegionsInfo { get; set; }

	/// <summary>
	/// The number of total devices, including both AWS and normal devices, that belong to this device group (includes normal devices in sub groups)
	/// </summary>
	[DataMember(Name = "numOfHosts", IsRequired = false)]
	public int NumOfHosts { get; set; }

	/// <summary>
	/// The number of sub-groups that belong only to this device group (doesn\u0027t include groups under sub-groups)
	/// </summary>
	[DataMember(Name = "numOfDirectSubGroups", IsRequired = false)]
	public string NumOfDirectSubGroups { get; set; }

	/// <summary>
	/// The number of AWS and normal devices that belong only to this device group (doesn\u0027t include devices in sub-groups)
	/// </summary>
	[DataMember(Name = "numOfDirectDevices", IsRequired = false)]
	public string NumOfDirectDevices { get; set; }

	/// <summary>
	/// The number of instances in each Azure region (only applies to Azure groups)
	/// </summary>
	[DataMember(Name = "azureRegionsInfo", IsRequired = false)]
	public string AzureRegionsInfo { get; set; }
}
