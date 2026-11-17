namespace LogicMonitor.Api;

/// <summary>
///     Resource Portal interaction
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	///     Gets Resources by ResourceGroup full path
	/// </summary>
	/// <param name="resourceGroupFullPaths">The FullPath(es) of the ResourceGroup(s), semicolon separated.</param>
	/// <param name="recurse">If true, finds devices in child groups also.</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <param name="resourceFilter">
	///     An optional filter applied to each per-group Resource fetch. Its main use is Properties,
	///     to request only the fields the caller reads: without it every property of every Resource
	///     is fetched, for every group. Null preserves the previous behaviour.
	/// </param>
	/// <returns>A list of Resources</returns>
	public async Task<List<Resource>> GetResourcesByResourceGroupFullPathAsync(
		string resourceGroupFullPaths,
		bool recurse,
		CancellationToken cancellationToken,
		[Optional] Filter<Resource>? resourceFilter)
	{
		var resources = new List<Resource>();

		foreach (var searchResourceGroupName in resourceGroupFullPaths.Split(';').Select(path => path.TrimStart()).ToList())
		{
			// Make sure the ResourceGroup exists
			if (await GetResourceGroupByFullPathAsync(searchResourceGroupName, cancellationToken).ConfigureAwait(false)
				is not ResourceGroup checkedResourceGroup)
			{
				continue;
			}

			var resourceGroups = await GetResourceGroupsToSearchAsync(checkedResourceGroup, searchResourceGroupName, recurse, cancellationToken).ConfigureAwait(false);

			foreach (var resourceGroup in resourceGroups)
			{
				var resourceGroupResources = await GetResourcesByResourceGroupIdAsync(
					resourceGroup.Id,
					resourceFilter,
					cancellationToken
					).ConfigureAwait(false);

				resources.AddRange(resourceGroupResources);
			}
		}

		return [.. resources.DistinctBy(d => d.Id)];
	}

	private async Task<List<ResourceGroup>> GetResourceGroupsToSearchAsync(
		ResourceGroup checkedResourceGroup,
		string searchResourceGroupName,
		bool recurse,
		CancellationToken cancellationToken)
	{
		var resourceGroups = await GetCandidateResourceGroupsAsync(checkedResourceGroup, recurse, cancellationToken).ConfigureAwait(false);

		// Ensure the one we actually found is included
		if (!resourceGroups.Exists(dg => dg.Id == checkedResourceGroup.Id))
		{
			resourceGroups.Add(checkedResourceGroup);
		}

		if (recurse && checkedResourceGroup.Id != 1)
		{
			// Includes<ResourceGroup> has no StartsWith, so drop the groups whose full path did not start with the searched-for group
			resourceGroups.RemoveAll(dg => !dg.FullPath.StartsWith(searchResourceGroupName, StringComparison.Ordinal));
		}

		return resourceGroups;
	}

	private Task<List<ResourceGroup>> GetCandidateResourceGroupsAsync(ResourceGroup checkedResourceGroup, bool recurse, CancellationToken cancellationToken)
	{
		if (checkedResourceGroup.Id == 1)
		{
			return recurse
				? GetAllAsync<ResourceGroup>(cancellationToken: cancellationToken)
				: GetAllAsync(
					new Filter<ResourceGroup>
					{
						FilterItems =
						[
							new Eq<ResourceGroup>(nameof(ResourceGroup.Id), 1)
						]
					},
					cancellationToken: cancellationToken);
		}

		if (!recurse)
		{
			// Issue #48: the group is already resolved, and re-querying it with Eq reintroduces the parentheses and whitespace problem the lookup worked around
			return Task.FromResult(new List<ResourceGroup> { checkedResourceGroup });
		}

		// LM API Includes filter (~) cannot handle parentheses and pipes
		if (checkedResourceGroup.FullPath.Contains('(')
			|| checkedResourceGroup.FullPath.Contains(')')
			|| checkedResourceGroup.FullPath.Contains('|'))
		{
			return GetAllSubResourceGroups(checkedResourceGroup.Id, cancellationToken);
		}

		return GetAllAsync(
			new Filter<ResourceGroup>
			{
				FilterItems =
				[
					new Includes<ResourceGroup>(nameof(ResourceGroup.FullPath), checkedResourceGroup.FullPath)
				]
			},
			cancellationToken: cancellationToken);
	}

	/// <summary>
	///     Get Resources by associated DataSource
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceWithDataSourceInstanceInformation>> GetResourcesAndInstancesAssociatedWithDataSourceByIdPageAsync(
		int dataSourceId,
		Filter<ResourceWithDataSourceInstanceInformation> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceWithDataSourceInstanceInformation>>($"setting/datasources/{dataSourceId}/devices?{filter}", cancellationToken);

	/// <summary>
	///     Get Resources by name
	/// </summary>
	/// <param name="searchString">The search string</param>
	/// <param name="maxResultCount">Max result count.  May not exceed 100</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<Resource>> GetResourcesByNameAsync(
		string searchString,
		int maxResultCount,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(searchString);

		var treeNodeFreeSearchResults = await TreeNodeFreeSearchAsync(
			searchString,
			maxResultCount,
			cancellationToken,
			TreeNodeFreeSearchResultType.Resource
			)
			.ConfigureAwait(false);

		var resources = new List<Resource>();
		foreach (var resourceResult in treeNodeFreeSearchResults)
		{
			resources.Add(await GetAsync<Resource>(resourceResult.EntityId, cancellationToken).ConfigureAwait(false));
		}

		return resources;
	}

	/// <summary>
	///     Whether a resource group full path must be resolved via a tree node search rather than an
	///     Eq filter, because LogicMonitor's filter endpoint cannot carry the value faithfully.
	/// </summary>
	internal static bool RequiresTreeNodeSearchFallback(string fullPath, string[] segments)
		=> fullPath.Contains('(')
			|| fullPath.Contains(')')
			|| Array.Exists(segments, segment => segment != segment.Trim());
}
