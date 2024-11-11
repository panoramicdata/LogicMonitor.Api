namespace LogicMonitor.Api.Resources;

/// <summary>
///    A ResourceGroup creation DTO
/// </summary>
[DataContract]
public class ResourceGroupCreationDto : CreationDto<ResourceGroup>, IHasName, IHasDescription
{
	/// <summary>
	///    The Parent Group Id as a string
	/// </summary>
	[DataMember(Name = "parentId")]
	public string ParentId { get; set; } = string.Empty;

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    Disable Alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///    What the ResourceGroup applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	///    Custom ResourceGroup properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
