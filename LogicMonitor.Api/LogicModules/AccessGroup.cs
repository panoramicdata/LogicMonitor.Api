namespace LogicMonitor.Api.LogicModules;

[DataContract]
public class AccessGroup
{
	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "createdOn")]
	public long CreatedOn { get; set; }

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "updatedOn")]
	public long UpdatedOn { get; set; }

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string? TenantId { get; set; }
}