namespace LogicMonitor.Api.Topologies;

/// <summary>
/// TopologyGroup creation DTO
/// </summary>
[DataContract]
public class TopologyGroupCreationDto : CreationDto<TopologyGroup>
{
	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;
}
