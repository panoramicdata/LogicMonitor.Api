namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan policy device group assignment
/// </summary>
[DataContract]
public class Assignment
{
	/// <summary>
	/// The name of the group that discovered devices should be added into
	/// </summary>
	[DataMember(Name = "groupName", IsRequired = false)]
	public string? GroupName { get; set; }

	/// <summary>
	/// tagValue
	/// </summary>
	[DataMember(Name = "tagValue", IsRequired = false)]
	public string? TagValue { get; set; }

	/// <summary>
	/// Whether or not specified devices should be included or excluded
	/// </summary>
	[DataMember(Name = "action", IsRequired = false)]
	public NetscanInclusionType InclusionType { get; set; }

	/// <summary>
	/// Whether or not alerting should be disabled for discovered devices
	/// </summary>
	[DataMember(Name = "disableAlerting", IsRequired = false)]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// tagKey
	/// </summary>
	[DataMember(Name = "tagKey", IsRequired = false)]
	public string? TagKey { get; set; }

	/// <summary>
	/// The ID of the group that discovered devices should be added into
	/// </summary>
	[DataMember(Name = "group", IsRequired = false)]
	public int DeviceGroupId { get; set; }
}
