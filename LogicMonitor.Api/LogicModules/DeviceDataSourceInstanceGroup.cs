namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DeviceDataSourceInstanceGroup
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceGroup : NamedItem
{
	/// <summary>
	/// time when the group was created
	/// </summary>
	[DataMember(Name = "createOn")]
	public long CreatedOnUtcSeconds { get; set; }

	/// <summary>
	/// The id of associated device
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The display name of the device
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// the device datasource id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }
}
