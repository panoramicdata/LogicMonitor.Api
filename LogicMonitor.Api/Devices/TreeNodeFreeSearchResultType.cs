namespace LogicMonitor.Api.Devices;

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
	/// Device
	/// </summary>
	[EnumMember(Value = "device")]
	Device = 2,

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
