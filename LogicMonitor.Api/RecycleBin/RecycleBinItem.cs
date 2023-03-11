namespace LogicMonitor.Api.RecycleBin;

/// <summary>
/// A recycle bin item
/// </summary>
[DataContract]
public class RecycleBinItem : IHasEndpoint
{
	/// <summary>
	/// The ID
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The Resource Type
	/// </summary>
	[DataMember(Name = "resourceType")]
	public string ResourceType { get; set; } = string.Empty;

	/// <summary>
	/// The deleted resource's name
	/// </summary>
	[DataMember(Name = "resourceName")]
	public string ResourceName { get; set; } = string.Empty;

	/// <summary>
	/// When it was deleted in milliseconds since the Epoch
	/// </summary>
	[DataMember(Name = "deletedOn")]
	public long DeletedOnMs { get; set; }

	/// <summary>
	/// When it was deleted in seconds since the Epoch
	/// </summary>
	public long DeletedOnSeconds => DeletedOnMs / 1000;

	/// <summary>
	/// Who deleted it
	/// </summary>
	[DataMember(Name = "deletedBy")]
	public string DeletedBy { get; set; } = string.Empty;

	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "resourceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The paths
	/// </summary>
	[DataMember(Name = "paths")]
	public List<string> Paths { get; set; } = new();

	/// <summary>
	/// The endpoint
	/// </summary>
	public string Endpoint() => "recyclebin/recycles";
}
