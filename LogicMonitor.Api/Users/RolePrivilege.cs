using System.Diagnostics;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Users;

/// <summary>
/// A Role privilege
/// </summary>
[DataContract]
[DebuggerDisplay("{ObjectType} {ObjectId} {ObjectName} {Operation} {SubOperation}")]
public class RolePrivilege
{
	/// <summary>
	/// The object type
	/// </summary>
	[DataMember(Name = "objectType")]
	public PrivilegeObjectType ObjectType { get; set; }

	/// <summary>
	/// The object id
	/// </summary>
	[DataMember(Name = "objectId")]
	public string ObjectId { get; set; }

	/// <summary>
	/// The object name
	/// </summary>
	[DataMember(Name = "objectName")]
	public string ObjectName { get; set; }

	/// <summary>
	/// The Operation
	/// </summary>
	[DataMember(Name = "operation")]
	public RolePrivilegeOperation Operation { get; set; }

	/// <summary>
	/// The Sub-operation
	/// </summary>
	[DataMember(Name = "subOperation")]
	public string SubOperation { get; set; }
}
