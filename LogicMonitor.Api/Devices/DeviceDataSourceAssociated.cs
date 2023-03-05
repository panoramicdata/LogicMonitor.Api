namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceAssociated
/// </summary>

[DataContract]
public class DeviceDataSourceAssociated : NamedItem
{
	/// <summary>
	/// The instance list associated to the datasource
	/// </summary>
	[DataMember(Name = "instance")]
	public List<DeviceDataSourceAssociatedInstance>? Instances { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Whether has more instance. 0 no more, 1 has more
	/// </summary>
	[DataMember(Name = "hasMore")]
	public int HasMore { get; set; }

	/// <summary>
	/// Whether has active instance
	/// </summary>
	[DataMember(Name = "hasActiveInstance")]
	public bool HasActiveInstance { get; set; }
}
