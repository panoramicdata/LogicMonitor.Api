namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceAssociatedInstance
/// </summary>

[DataContract]
public class DeviceDataSourceAssociatedInstance : NamedItem
{
	/// <summary>
	/// instance alias
	/// </summary>
	[DataMember(Name = "alias")]
	public string Alias { get; set; }
}
