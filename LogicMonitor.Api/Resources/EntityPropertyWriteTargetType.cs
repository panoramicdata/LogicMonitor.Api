namespace LogicMonitor.Api.Resources;

/// <summary>
///    The type of entity that an <see cref="EntityPropertyWrite" /> targets.
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum EntityPropertyWriteTargetType
{
	/// <summary>
	///    Unknown / unset.
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    A <see cref="Resource" /> (device).
	/// </summary>
	[EnumMember(Value = "resource")]
	Resource,

	/// <summary>
	///    A <see cref="ResourceGroup" /> (device group).
	/// </summary>
	[EnumMember(Value = "resourceGroup")]
	ResourceGroup
}
