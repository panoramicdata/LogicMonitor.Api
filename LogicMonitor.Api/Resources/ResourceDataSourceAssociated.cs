namespace LogicMonitor.Api.Resources;

/// <summary>
/// ResourceDataSourceAssociated
/// </summary>

[DataContract]
public class ResourceDataSourceAssociated : NamedItem
{
	/// <summary>
	/// The instance list associated to the datasource
	/// </summary>
	[DataMember(Name = "instance")]
	public List<ResourceDataSourceAssociatedInstance>? Instances { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Whether has more instance. 0 no more, 1 has more
	/// </summary>
	[DataMember(Name = "hasMore")]
	public int HasMore { get; set; }

	/// <summary>
	/// Whether has active instance
	/// </summary>
	[DataMember(Name = "hasActiveInstance")]
	public bool HasActiveInstance { get; set; }
}
