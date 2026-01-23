namespace LogicMonitor.Api.Attributes;

/// <summary>
/// Specifies alternative string values for enum member deserialization.
/// Use alongside [EnumMember] when an API may return multiple string representations.
/// </summary>
/// <remarks>
/// Creates a new EnumMemberAliasAttribute with the specified aliases.
/// </remarks>
/// <param name="aliases">Alternative string values for deserialization.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class EnumMemberAliasAttribute(params string[] aliases) : Attribute
{
	/// <summary>
	/// Gets the alternative string values that should deserialize to this enum member.
	/// </summary>
	public string[] Aliases { get; } = aliases ?? [];
}
