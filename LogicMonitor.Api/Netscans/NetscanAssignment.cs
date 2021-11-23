namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan policy device group assignment
/// </summary>
[DataContract]
public class NetscanAssignment
{
	/// <summary>
	///    The netscan policy assignment type
	/// </summary>
	[DataMember(Name = "type")]
	public NetscanAssignmentType Type { get; set; }

	/// <summary>
	///    The query (used for custom NetscanPolicyAssignmentType)
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; }

	/// <summary>
	///    Whether to disable alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///    The device group to assign this to
	/// </summary>
	[DataMember(Name = "group")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///    The device group name to assign this to
	/// </summary>
	[DataMember(Name = "groupName")]
	public string DeviceGroupName { get; set; }

	/// <summary>
	///    The device group to assign this to
	/// </summary>
	[DataMember(Name = "action")]
	public NetscanInclusionType InclusionType { get; set; }
}
