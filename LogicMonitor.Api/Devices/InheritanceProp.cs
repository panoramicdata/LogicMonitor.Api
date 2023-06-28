namespace LogicMonitor.Api.Devices;

/// <summary>
/// InheritanceProp
/// </summary>

[DataContract]
public class InheritanceProp : IdentifiedItem
{
	/// <summary>
	/// The fullpath of the property
	/// </summary>
	[DataMember(Name = "fullpath")]
	public string Fullpath { get; set; } = string.Empty;

	/// <summary>
	/// The type of the parent property from which are inheriting
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The property value for the group
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
