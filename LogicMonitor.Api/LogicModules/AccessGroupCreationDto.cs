namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A request to create an access group
/// </summary>
[DataContract]
public class AccessGroupCreationDto : CreationDto<AccessGroup>
{
	/// <summary>
	/// The access group name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The access group description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string? TenantId { get; set; }
}
