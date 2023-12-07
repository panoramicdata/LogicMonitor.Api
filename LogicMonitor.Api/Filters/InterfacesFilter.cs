namespace LogicMonitor.Api.Filters;

/// <summary>
/// Interfaces Filter
/// </summary>

[DataContract]
public class InterfacesFilter
{
	/// <summary>
	/// interfaceTypes
	/// </summary>
	[DataMember(Name = "interfaceTypes")]
	public List<InterfaceType> InterfaceTypes { get; set; } = [];

	/// <summary>
	/// interfaceTypes
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
