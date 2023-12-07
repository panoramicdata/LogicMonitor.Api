namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A dashboard group creation dto
/// </summary>
[DataContract]
public class DashboardGroupCreationDto : CreationDto<DashboardGroup>
{
	/// <summary>
	///    The Parent Group Id
	/// </summary>
	[DataMember(Name = "parentId")]
	public string ParentId { get; set; } = "1";

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    The tokens
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<SimpleProperty> CustomProperties { get; set; } = [];

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
