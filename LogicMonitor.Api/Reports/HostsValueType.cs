namespace LogicMonitor.Api.Reports;

/// <summary>
/// A hosts value type
/// </summary>
[DataContract]
public enum HostsValueType
{
	/// <summary>
	/// Group
	/// </summary>
	[EnumMember(Value = "group")]
	Group = 0,

	/// <summary>
	/// Host
	/// </summary>
	[EnumMember(Value = "host")]
	Host = 1,
}