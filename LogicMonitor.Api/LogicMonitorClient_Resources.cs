namespace LogicMonitor.Api;

/// <summary>
///     Resource Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	private async Task<List<ResourceGroup>> GetAllSubResourceGroups(
		int resourceGroupId,
		CancellationToken cancellationToken)
	{
		var allResourceGroups = new List<ResourceGroup>();

		var resourceGroups =
			await GetAllAsync(
				new Filter<ResourceGroup>
				{
					FilterItems =
					[
						new Eq<ResourceGroup>(nameof(ResourceGroup.ParentId), resourceGroupId)
					]
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (resourceGroups.Count != 0)
		{
			foreach (var resourceGroup in resourceGroups)
			{
				allResourceGroups.Add(resourceGroup);
				allResourceGroups.AddRange(await GetAllSubResourceGroups(resourceGroup.Id, cancellationToken).ConfigureAwait(false));
			}
		}

		return allResourceGroups;
	}

	/// <summary>
	///     Gets Resources by HostName
	/// </summary>
	/// <param name="hostName">The Resource HostName</param>
	/// <param name="maxResultCount">Max result count.  May not exceed 100</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<Resource>> GetResourcesByHostNameAsync
	(string hostName, int maxResultCount, CancellationToken cancellationToken)
		=> [.. (await GetResourcesByNameAsync(
			hostName,
			maxResultCount,
			cancellationToken).ConfigureAwait(false))
			.Where(d => string.Equals(d.Name, hostName, StringComparison.OrdinalIgnoreCase))];

	/// <summary>
	///     Gets Resource by DisplayName
	/// </summary>
	/// <param name="displayName">The Resource DisplayName</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Resource?> GetResourceByDisplayNameAsync(
		string displayName,
		CancellationToken cancellationToken)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

		// The LM API Eq filter cannot handle parentheses in values - fall back to tree node search
		if (displayName.Contains('(') || displayName.Contains(')'))
		{
			return (await GetResourcesByNameAsync(
				displayName,
				100,
				cancellationToken)
				.ConfigureAwait(false))
				.SingleOrDefault(r => r.DisplayName == displayName);
		}

		return (await GetAllAsync(new Filter<Resource>
		{
			FilterItems =
			[
				new Eq<Resource>(nameof(Resource.DisplayName), displayName
					.EscapeSlashes()
					.EscapePlusCharacter())
			]
		},
		cancellationToken)
		.ConfigureAwait(false))
		.SingleOrDefault(r => r.DisplayName == displayName);
	}

	/// <summary>
	///     Get device properties, in the following order:
	///     - Custom
	///     - Auto
	///     - System
	///     - Inherit
	/// </summary>
	/// <param name="resourceId">The Resource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetResourcePropertiesAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"device/devices/{resourceId}/properties", cancellationToken);

	/// <summary>
	/// Schedule active discovery for a Resource
	/// </summary>
	/// <param name="resourceId">The Resource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task ScheduleActiveDiscovery(
		int resourceId,
		CancellationToken cancellationToken)
		=> PostAsync<object, object>(
			new object(),
			$"device/devices/{resourceId}/scheduleAutoDiscovery",
			cancellationToken);

	/// <summary>
	///     Get Resource alerts
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="skip">The number to skip</param>
	/// <param name="take">The number to take</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Alert>> GetResourceAlertsPageAsync(
		int resourceId,
		int skip,
		int take,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Alert>>($"device/devices/{resourceId}/alerts?size={take}&offset={skip}", cancellationToken);

	/// <summary>
	///     Set single resource property
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceCustomPropertyAsync(
		int resourceId,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	=>
		SetCustomPropertyAsync(
			resourceId,
			name,
			value,
			mode,
			"device/devices",
			cancellationToken
			);

	/// <summary>
	///     Set single Resource property, using automatic mode
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceCustomPropertyAsync(
		int resourceId,
		string name,
		string? value,
		CancellationToken cancellationToken)
		=> SetCustomPropertyAsync(
			resourceId,
			name,
			value,
			SetPropertyMode.Automatic,
			"device/devices",
			cancellationToken
			);

	/// <summary>
	///     Set single ResourceGroup property
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceGroupCustomPropertyAsync(
		int resourceGroupId,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	=>
		SetCustomPropertyAsync(
			resourceGroupId,
			name,
			value,
			mode,
			"device/groups",
			cancellationToken);

	/// <summary>
	///     Applies a single <see cref="EntityPropertyWrite" />, writing one custom property onto the
	///     target <see cref="Resource" /> or <see cref="ResourceGroup" />.
	/// </summary>
	/// <remarks>
	///     This writes only the named property (never a full object), so you always supply the real value
	///     rather than round-tripping a masked <c>********</c>. It is the admin-gated way to set or rotate
	///     hidden fields such as <c>snmp.community</c>, <c>*.pass</c> and <c>*.key</c>. (For reference: a
	///     full-object PUT does not clobber a Resource/ResourceGroup secret either - the server treats the
	///     <c>********</c> mask as "unchanged" - but this method is how you change the stored value.)
	/// </remarks>
	/// <param name="write">The property write directive.</param>
	/// <param name="mode">How to set the property (Create, Update, Delete or Automatic).</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="NotSupportedException">Thrown when the write targets an unsupported entity type.</exception>
	public Task SetCustomPropertyAsync(
		EntityPropertyWrite write,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(write);
		return write.Type switch
		{
			EntityPropertyWriteTargetType.Resource
				=> SetResourceCustomPropertyAsync(write.Id, write.Name, write.Value, mode, cancellationToken),
			EntityPropertyWriteTargetType.ResourceGroup
				=> SetResourceGroupCustomPropertyAsync(write.Id, write.Name, write.Value, mode, cancellationToken),
			_ => throw new NotSupportedException($"Unsupported {nameof(EntityPropertyWriteTargetType)}: {write.Type}")
		};
	}

	/// <summary>
	///     Applies a batch of <see cref="EntityPropertyWrite" /> directives in order. This is the
	///     config-as-code entry point for setting hidden property fields across resources and groups.
	/// </summary>
	/// <param name="writes">The property write directives to apply, in order.</param>
	/// <param name="mode">How to set each property (Create, Update, Delete or Automatic).</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public async Task SetCustomPropertiesAsync(
		IEnumerable<EntityPropertyWrite> writes,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(writes);
		foreach (var write in writes)
		{
			await SetCustomPropertyAsync(write, mode, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Gets Resources
	/// </summary>
	/// <param name="filter">The Resource filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Resource>> GetResourcesPageAsync(
		Filter<Resource> filter,
		CancellationToken cancellationToken)
	{
		if (filter is not null && filter.Order is null)
		{
			filter.Order = new Order<Resource> { Property = nameof(Resource.Id), Direction = OrderDirection.Asc };
		}

		return GetBySubUrlAsync<Page<Resource>>($"device/devices?{filter}", cancellationToken);
	}

	/// <summary>
	///     Gets Resources by ResourceGroup Id
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<Resource>> GetResourcesByResourceGroupIdAsync(
		int resourceGroupId,
		Filter<Resource>? filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(filter, $"device/groups/{resourceGroupId}/devices", cancellationToken);

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
