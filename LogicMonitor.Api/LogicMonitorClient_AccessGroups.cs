namespace LogicMonitor.Api;

/// <summary>
///     Access group portal interaction
/// </summary>
/// <remarks>Access group CRUD uses the generic methods, for example GetAllAsync&lt;AccessGroup&gt; and CreateAsync(new AccessGroupCreationDto())</remarks>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Map or unmap LogicModules to access groups: each item sets the full list of access groups for one module
	/// </summary>
	/// <param name="mappingDetails">The module to access group mappings</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The ids that succeeded and the failures</returns>
	public Task<AccessGroupModuleMapping> MapModulesToAccessGroupsAsync(
		List<AccessGroupModuleMappingItem> mappingDetails,
		CancellationToken cancellationToken)
		=> PostAsync<AccessGroupModuleMapping, AccessGroupModuleMapping>(
			new AccessGroupModuleMapping { MappingDetails = mappingDetails },
			"setting/accessgroup/mapunmap/modules",
			cancellationToken);
}
