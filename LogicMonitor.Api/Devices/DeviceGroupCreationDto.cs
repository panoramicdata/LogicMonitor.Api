namespace LogicMonitor.Api.Devices;

/// <summary>
///    A device group creation dto
/// </summary>
[DataContract]
public class DeviceGroupCreationDto : CreationDto<DeviceGroup>
{
	/// <summary>
	///    The Parent Group Id as a string
	/// </summary>
	[DataMember(Name = "parentId")]
	public string ParentId { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    Disable Alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///    What the DeviceGroup applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; }

	/// <summary>
	///    Custom DeviceGroup properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; }

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
