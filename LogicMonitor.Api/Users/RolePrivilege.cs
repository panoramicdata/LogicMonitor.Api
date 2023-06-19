namespace LogicMonitor.Api.Users;

/// <summary>
/// A Role privilege
/// </summary>
[DataContract]
[DebuggerDisplay("{ObjectType} {ObjectId} {ObjectName} {Operation} {SubOperation}")]
public class RolePrivilege
{
	/// <summary>
	/// The privilege object type. values can be [dashboard_group|dashboard|host_group|service_group|website_group|report_group|remoteSession|chat|setting|device_dashboard|help|logs|configNeedDeviceManagePermission|map|resourceMapTab|tracesManageTab]
	/// </summary>
	[DataMember(Name = "objectType")]
	public PrivilegeObjectType ObjectType { get; set; }

	/// <summary>
	/// The privilege object identifier
	/// </summary>
	[DataMember(Name = "objectId")]
	public string? ObjectId { get; set; }

	/// <summary>
	/// The privilege object name
	/// </summary>
	[DataMember(Name = "objectName")]
	public string? ObjectName { get; set; }

	/// <summary>
	/// The privilege operation
	/// </summary>
	[DataMember(Name = "operation")]
	public RolePrivilegeOperation Operation { get; set; }

	/// <summary>
	/// The highest privilege operation on its children operations
	/// </summary>
	[DataMember(Name = "subOperation")]
	public string? SubOperation { get; set; }
}
