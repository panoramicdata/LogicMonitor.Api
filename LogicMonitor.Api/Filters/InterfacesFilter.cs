namespace LogicMonitor.Api.Filters;

/// <summary>
/// Interfaces Filter
/// </summary>

[DataContract]
public class InterfacesFilter
{
	/// <summary>
	/// interface Types
	/// </summary>
	[DataMember(Name = "interfaceTypes")]
	public List<InterfaceType> InterfaceTypes { get; set; } = [];

	/// <summary>
	/// Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
