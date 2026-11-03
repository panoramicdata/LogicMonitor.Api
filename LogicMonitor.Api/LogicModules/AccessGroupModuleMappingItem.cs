namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The access groups to map a single LogicModule to
/// </summary>
[DataContract]
public class AccessGroupModuleMappingItem
{
	/// <summary>
	/// Id's of the access group
	/// </summary>
	[DataMember(Name = "accessgroups")]
	public List<int> Accessgroups { get; set; } = [];

	/// <summary>
	/// Id of the logic module
	/// </summary>
	[DataMember(Name = "moduleid")]
	public int Moduleid { get; set; }

	/// <summary>
	/// Type of module
	/// </summary>
	[DataMember(Name = "moduletype")]
	public string Moduletype { get; set; } = string.Empty;
}
