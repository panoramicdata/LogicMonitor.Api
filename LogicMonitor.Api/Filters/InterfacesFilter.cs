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
	[DataMember(Name = "interfaceTypes", IsRequired = false)]
	public List<InterfaceType> InterfaceTypes { get; set; } = new();

	/// <summary>
	/// interfaceTypes
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int DeviceId { get; set; }
}
