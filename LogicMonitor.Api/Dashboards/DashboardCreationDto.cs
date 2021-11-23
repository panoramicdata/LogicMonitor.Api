using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A dashboard creation dto
/// </summary>
[DataContract]
public class DashboardCreationDto : CreationDto<Dashboard>
{
	/// <summary>
	///    The Parent Group Id as a string
	/// </summary>
	[DataMember(Name = "groupId")]
	public string ParentId { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    Whether the dashboard group is shareable
	/// </summary>
	[DataMember(Name = "sharable")]
	public bool Sharable { get; set; }

	/// <summary>
	///    Whether to use dynamic widgets
	/// </summary>
	[DataMember(Name = "useDynamicWidget")]
	public bool UseDynamicWidgets { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
