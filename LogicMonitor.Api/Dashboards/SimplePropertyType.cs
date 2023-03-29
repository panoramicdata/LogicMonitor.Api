namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    The types of simple property
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum SimplePropertyType : byte
{
	/// <summary>
	///    Inherit
	/// </summary>
	[EnumMember(Value = "inherit")] Inherit,

	/// <summary>
	///    Owned
	/// </summary>
	[EnumMember(Value = "owned")] Owned
}
