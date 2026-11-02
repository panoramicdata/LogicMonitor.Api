namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A request to map or unmap LogicModules to access groups, and its outcome
/// </summary>
[DataContract]
public class AccessGroupModuleMapping
{
	/// <summary>
	/// The Module Id's not mapped successfully
	/// </summary>
	[DataMember(Name = "failure")]
	public List<string> Failure { get; set; } = [];

	/// <summary>
	/// Can provide module id, module type and access group for mapping
	/// </summary>
	[DataMember(Name = "mappingDetails")]
	public List<AccessGroupModuleMappingItem> MappingDetails { get; set; } = [];

	/// <summary>
	/// The Module Id's mapped successfully
	/// </summary>
	[DataMember(Name = "success")]
	public List<int> Success { get; set; } = [];
}
