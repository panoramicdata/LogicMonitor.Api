using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
/// The way in which to set the property
/// </summary>
public enum SetPropertyMode
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Create, update or delete as appropriate
	/// </summary>
	Automatic,

	/// <summary>
	/// Create
	/// </summary>
	Create,

	/// <summary>
	/// Update
	/// </summary>
	Update,

	/// <summary>
	/// Delete
	/// </summary>
	Delete
}
