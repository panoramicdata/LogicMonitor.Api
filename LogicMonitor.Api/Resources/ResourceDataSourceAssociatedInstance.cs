namespace LogicMonitor.Api.Resources;

/// <summary>
/// ResourceDataSourceAssociatedInstance
/// </summary>
[DataContract]
public class ResourceDataSourceAssociatedInstance : NamedItem
{
	/// <summary>
	/// instance alias
	/// </summary>
	[DataMember(Name = "alias")]
	public string Alias { get; set; } = string.Empty;
}
