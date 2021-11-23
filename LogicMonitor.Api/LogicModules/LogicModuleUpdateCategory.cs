using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogicModule update category
/// </summary>
[DataContract]
public enum LogicModuleUpdateCategory
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// New
	/// </summary>
	[EnumMember(Value = "new")]
	New,

	/// <summary>
	/// Updated not in use
	/// </summary>
	[EnumMember(Value = "updatedNotInUse")]
	UpdatedNotInUse,

	/// <summary>
	/// Updated in use
	/// </summary>
	[EnumMember(Value = "updatedInUse")]
	UpdatedInUse,

	/// <summary>
	/// audited
	/// </summary>
	[EnumMember(Value = "audited")]
	Audited
}
