namespace LogicMonitor.Api.Devices;

/// <summary>
/// Device Property
/// </summary>

[DataContract]
public class DeviceProperty
{
	/// <summary>
	/// The resource property name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } 

	/// <summary>
	/// The resource property value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } 
}
