namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An access group
/// </summary>
[DataContract]
public class AccessGroup : IdentifiedItem, IHasEndpoint, IHasCreationEndpoint, IPatchable
{
	/// <summary>
	///		The access group name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///		When the access group was created, in milliseconds since the epoch
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdOn")]
	public long CreatedOn { get; set; }

	/// <summary>
	///		When the access group was last updated, in milliseconds since the epoch
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "updatedOn")]
	public long UpdatedOn { get; set; }

	/// <summary>
	///		The user who created the access group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	///		The access group description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///		The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string? TenantId { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/accessgroup";

	/// <inheritdoc />
	public string CreationEndpoint() => "setting/accessgroup/add";
}
