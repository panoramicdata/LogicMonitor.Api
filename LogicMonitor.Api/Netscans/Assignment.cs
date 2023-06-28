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
	[DataMember(Name = "groupName")]
	public string? GroupName { get; set; }

	/// <summary>
	/// tagValue
	/// </summary>
	[DataMember(Name = "tagValue")]
	public string? TagValue { get; set; }

	/// <summary>
	/// Whether or not specified devices should be included or excluded
	/// </summary>
	[DataMember(Name = "action")]
	public NetscanInclusionType InclusionType { get; set; }

	/// <summary>
	/// Whether or not alerting should be disabled for discovered devices
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// tagKey
	/// </summary>
	[DataMember(Name = "tagKey")]
	public string? TagKey { get; set; }

	/// <summary>
	/// The ID of the group that discovered devices should be added into
	/// </summary>
	[DataMember(Name = "group")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type")]
	public NetscanAssignmentType Type { get; set; }

	/// <summary>
	/// query
	/// </summary>
	[DataMember(Name = "query")]
	public string? Query { get; set; }
}
