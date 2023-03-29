namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A dashboard group property type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum DashboardGroupPropertyType : byte
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
