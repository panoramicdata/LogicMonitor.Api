namespace LogicMonitor.Api.Resources;

/// <summary>
/// A device type
/// </summary>
[DataContract]
public enum TreeNodeFreeSearchResultType
{
	/// <summary>
	/// Unknown Resource Type
	/// </summary>
	[EnumMember(Value = "Unknown")]
	Unknown = 0,

	/// <summary>
	/// ResourceGroup
	/// </summary>
	[EnumMember(Value = "deviceGroup")]
	ResourceGroup = 1,

	/// <summary>
	/// Resource
	/// </summary>
	[EnumMember(Value = "device")]
	Resource = 2,

	/// <summary>
	/// ResourceDataSource
	/// </summary>
	[EnumMember(Value = "deviceDataSource")]
	ResourceDataSource = 3,

	/// <summary>
	/// ResourceDataSourceInstance
	/// </summary>
	[EnumMember(Value = "deviceDataSourceInstance")]
	ResourceDataSourceInstance = 4,
}
