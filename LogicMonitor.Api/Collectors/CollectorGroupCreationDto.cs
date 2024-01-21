namespace LogicMonitor.Api.Collectors;

/// <summary>
///     A LogicMonitor Collector Group creation DTO
/// </summary>
[DataContract]
public class CollectorGroupCreationDto : CreationDto<CollectorGroup>, IHasName, IHasDescription
{
	/// <summary>
	///     The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///     The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///     The custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];
}
