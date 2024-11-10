namespace LogicMonitor.Api.Resources;

/// <summary>
/// A device type
/// </summary>
[DataContract]
public enum TreeNodeFreeSearchResultType
{
	/// <summary>
	/// Unknown Device Type
	/// </summary>
	[EnumMember(Value = "Unknown")]
	Unknown = 0,

	/// <summary>
	/// DeviceGroup
	/// </summary>
	[EnumMember(Value = "deviceGroup")]
	DeviceGroup = 1,

	/// <summary>
	/// Resource
	/// </summary>
	[EnumMember(Value = "device")]
	Resource = 2,

	/// <summary>
	/// DeviceDataSource
	/// </summary>
	[EnumMember(Value = "deviceDataSource")]
	DeviceDataSource = 3,

	/// <summary>
	/// DeviceDataSourceInstance
	/// </summary>
	[EnumMember(Value = "deviceDataSourceInstance")]
	DeviceDataSourceInstance = 4,
}
