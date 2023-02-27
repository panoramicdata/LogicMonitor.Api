namespace LogicMonitor.Api.Devices;

/// <summary>
/// InterfaceType
/// </summary>
[DataContract]
public class InterfaceType
{
	/// <summary>
	/// ifPosition
	/// </summary>
	[DataMember(Name = "ifPosition")]
	public string IfPosition { get; set; } = string.Empty;

	/// <summary>
	/// ifId
	/// </summary>
	[DataMember(Name = "ifId")]
	public string IfId { get; set; } = string.Empty;
}
