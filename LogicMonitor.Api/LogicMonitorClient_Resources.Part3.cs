namespace LogicMonitor.Api;

/// <summary>
///     Resource Portal interaction
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	///     Gets Resources by ResourceGroup full path
	/// </summary>
	/// <param name="resourceGroupFullPath"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroup?> GetResourceGroupByFullPathAsync(
		string resourceGroupFullPath,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(resourceGroupFullPath) || resourceGroupFullPath == "/")
		{
			return await GetAsync<ResourceGroup>(1, cancellationToken).ConfigureAwait(false);
		}

		// The LM API Eq filter cannot carry certain values - fall back to a tree node search and match
		// the full path client-side.
		//
		// Parentheses: the filter cannot handle them at all.
		//
		// Issue #40 - leading/trailing whitespace: LogicMonitor silently TRIMS whitespace in filter
		// values, so an Eq on a path whose segment ends in a space matches nothing and this method
		// returned null for a group that plainly exists. Verified against a live portal: the filter
		// returns 0 results for such a path and 1 for an otherwise identical control, while the free
		// search below returns the group correctly with its whitespace intact. Tested segment by
		// segment rather than on the whole path, because an interior segment can carry the space
		// ("A /B") without the full path looking untrimmed.
		var pathSegments = resourceGroupFullPath.Split('/');
		if (RequiresTreeNodeSearchFallback(resourceGroupFullPath, pathSegments))
		{
			// Search by leaf group name, then match by full path client-side
			var leafName = pathSegments[^1];
			var searchResults = await TreeNodeFreeSearchAsync(
				leafName,
				100,
				cancellationToken,
				TreeNodeFreeSearchResultType.ResourceGroup)
				.ConfigureAwait(false);

			foreach (var result in searchResults)
			{
				var group = await GetAsync<ResourceGroup>(result.EntityId, cancellationToken).ConfigureAwait(false);
				if (group.FullPath == resourceGroupFullPath)
				{
					return group;
				}
			}

			return null;
		}

		// This may actually return more than one, as LogicMonitor does not correctly handle brackets in some cases.
		var allResourceGroups =
			await GetAllAsync(new Filter<ResourceGroup>{
				FilterItems =
				[
					new Eq<ResourceGroup>(nameof(ResourceGroup.FullPath), resourceGroupFullPath.EscapePlusCharacter())
				]},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return allResourceGroups.SingleOrDefault(dg => dg.FullPath == resourceGroupFullPath);
	}

	/// <summary>
	///     Gets ResourceGroup properties
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetResourceGroupPropertiesAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"device/groups/{resourceGroupId}/properties", cancellationToken);

	/// <summary>
	/// Get ResourceGroup property by name
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	public Task<EntityProperty> GetResourceGroupPropertiesByNameAsync(
		int resourceGroupId,
		string name,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<EntityProperty>($"device/groups/{resourceGroupId}/properties/{name}", cancellationToken);

	/// <summary>
	/// Delete ResourceGroup property
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	public Task DeleteResourceGroupPropertyAsync(
		int resourceGroupId,
		string name,
		CancellationToken cancellationToken)
		=> DeleteAsync($"device/groups/{resourceGroupId}/properties/{name}", cancellationToken);

	/// <summary>
	///     Tree node free search
	/// </summary>
	/// <param name="searchText"></param>
	/// <param name="maxResultCount"></param>
	/// <param name="treeNodeFreeSearchResultType"></param>
	public Task<List<TreeNodeFreeSearchResult>> TreeNodeFreeSearchAsync(
		string searchText,
		int maxResultCount,
		[Optional] TreeNodeFreeSearchResultType? treeNodeFreeSearchResultType)
		=> TreeNodeFreeSearchAsync(searchText, maxResultCount, CancellationToken.None, treeNodeFreeSearchResultType);

	/// <summary>
	///     Tree node free search
	/// </summary>
	/// <param name="searchText"></param>
	/// <param name="maxResultCount"></param>
	/// <param name="treeNodeFreeSearchResultType"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<TreeNodeFreeSearchResult>> TreeNodeFreeSearchAsync(
		string searchText,
		int maxResultCount,
		CancellationToken cancellationToken,
		[Optional] TreeNodeFreeSearchResultType? treeNodeFreeSearchResultType)
	{
		ArgumentNullException.ThrowIfNull(searchText);

		var modifier = treeNodeFreeSearchResultType switch
		{
			TreeNodeFreeSearchResultType.ResourceDataSourceInstance => "i:",
			TreeNodeFreeSearchResultType.ResourceDataSource => "ds:",
			TreeNodeFreeSearchResultType.Resource => "d:",
			TreeNodeFreeSearchResultType.ResourceGroup => "g:",
			null => string.Empty,
			_ => throw new ArgumentException($"Unexpected value {treeNodeFreeSearchResultType}", nameof(treeNodeFreeSearchResultType)),
		};
		var treeNodeFreeSearchRequest = new TreeNodeFreeSearchRequest
		{
			Type = TreeNodeFreeSearchRequestType.TreeNodeFreeSearch,
			SearchText = $"{modifier}\"{searchText.EscapeProblematicCharacters()}\"",
			ResultLimitation = maxResultCount
		};

		return (await PostAsync<TreeNodeFreeSearchRequest, Page<TreeNodeFreeSearchResult>>(treeNodeFreeSearchRequest, "functions", cancellationToken).ConfigureAwait(false)).Items;
	}
}
