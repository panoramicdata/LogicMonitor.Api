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
		=> (await GetResourcesByNameAsync(
			hostName,
			maxResultCount,
			cancellationToken).ConfigureAwait(false))
			.Where(d => string.Equals(d.Name, hostName, StringComparison.OrdinalIgnoreCase)).ToList();

	/// <summary>
	///     Gets Resource by DisplayName
	/// </summary>
	/// <param name="displayName">The Resource DisplayName</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Resource> GetResourceByDisplayNameAsync(
		string displayName,
		CancellationToken cancellationToken)
		=> (await GetAllAsync(new Filter<Resource>
		{
			FilterItems =
				[
						new Eq<Resource>(nameof(Resource.DisplayName), (displayName ?? throw new ArgumentNullException(nameof(displayName)))
							.EscapeSlashes()
							.EscapePlusCharacter())
				]
		}, cancellationToken)
			   .ConfigureAwait(false))
			   .SingleOrDefault();

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
	/// <returns>A list of Resources</returns>
	public async Task<List<Resource>> GetResourcesByResourceGroupFullPathAsync(
		string resourceGroupFullPaths,
		bool recurse,
		CancellationToken cancellationToken)
	{
		var resources = new List<Resource>();

		foreach (var searchResourceGroupName in resourceGroupFullPaths.Split(';').Select(path => path.TrimStart()).ToList())
		{
			// Make sure the ResourceGroup exists
			if (await GetResourceGroupByFullPathAsync(searchResourceGroupName, cancellationToken).ConfigureAwait(false)
				is ResourceGroup checkedResourceGroup)
			{
				List<ResourceGroup> resourceGroups;

				// The root
				if (checkedResourceGroup.Id == 1)
				{
					resourceGroups =
						recurse
							// All ResourceGroups
							? await GetAllAsync<ResourceGroup>(cancellationToken: cancellationToken)
								.ConfigureAwait(false)
							// Only the root
							: await GetAllAsync(
								new Filter<ResourceGroup>
								{
									FilterItems =
									[
										new Eq<ResourceGroup>(nameof(ResourceGroup.Id), 1)
									]
								},
								cancellationToken: cancellationToken)
							.ConfigureAwait(false);
				}
				else
				{
					if (recurse)
					{
						if (!checkedResourceGroup.FullPath.Contains('(') &&
							!checkedResourceGroup.FullPath.Contains(')') &&
							!checkedResourceGroup.FullPath.Contains('|'))
						{
							resourceGroups =
								await GetAllAsync(
									new Filter<ResourceGroup>
									{
										FilterItems =
										[
											new Includes<ResourceGroup>(nameof(ResourceGroup.FullPath), checkedResourceGroup.FullPath)
										]
									},
									cancellationToken: cancellationToken)
								.ConfigureAwait(false);
						}
						else
						{
							// LM API Includes filter (~) cannot handle parentheses and pipes
							resourceGroups = await GetAllSubResourceGroups(
								checkedResourceGroup.Id,
								cancellationToken)
								.ConfigureAwait(false);
						}
					}
					else
					{
						resourceGroups =
							await GetAllAsync(
								new Filter<ResourceGroup>
								{
									FilterItems =
									[
											new Eq<ResourceGroup>(nameof(ResourceGroup.FullPath), searchResourceGroupName)
									]
								},
								cancellationToken: cancellationToken)
							.ConfigureAwait(false);
					}
				}

				// Ensure the one we actually found is included
				if (!resourceGroups.Exists(dg => dg.Id == checkedResourceGroup.Id))
				{
					resourceGroups.Add(checkedResourceGroup);
				}

				if (recurse && checkedResourceGroup.Id != 1)
				{
					// Filter out the ones where the full path did not START with the searched-for group, as we could
					// only use a Includes<ResourceGroup> filter and not a StartsWith (there isn't one!)
					resourceGroups.RemoveAll(dg => !dg.FullPath.StartsWith(searchResourceGroupName, StringComparison.Ordinal));
				}

				// Get the Resource
				foreach (var resourceGroup in resourceGroups)
				{
					var resourceGroupResources = await GetResourcesByResourceGroupIdAsync(
						resourceGroup.Id,
						null,
						cancellationToken
						).ConfigureAwait(false);

					resources.AddRange(resourceGroupResources);
				}
			}
		}

		return resources
			.DistinctBy(d => d.Id)
			.ToList();
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
		if (searchString is null)
		{
			throw new ArgumentNullException(nameof(searchString));
		}

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
	///     Gets Resources by ResourceGroup full path
	/// </summary>
	/// <param name="resourceGroupFullPath"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroup> GetResourceGroupByFullPathAsync(
		string resourceGroupFullPath,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(resourceGroupFullPath) || resourceGroupFullPath == "/")
		{
			return await GetAsync<ResourceGroup>(1, cancellationToken).ConfigureAwait(false);
		}

		// This may actually return more than one, as LogicMonitor does not correctly handle brackets in some cases.
		var allResourceGroups = await GetAllAsync(new Filter<ResourceGroup>
		{
			FilterItems =
			[
				new Eq<ResourceGroup>(nameof(ResourceGroup.FullPath), resourceGroupFullPath.EscapePlusCharacter().EscapeParens())
			]
		},
		cancellationToken: cancellationToken)
		.ConfigureAwait(false);

		return allResourceGroups
			.SingleOrDefault(dg => dg.FullPath == resourceGroupFullPath);
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
		TreeNodeFreeSearchResultType? treeNodeFreeSearchResultType = null)
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
		TreeNodeFreeSearchResultType? treeNodeFreeSearchResultType = null)
	{
		if (searchText is null)
		{
			throw new ArgumentNullException(nameof(searchText));
		}

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

	/// <summary>
	///     Gets the full Resource tree
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroup> GetFullResourceTreeAsync(
		int resourceGroupId = -1,
		CancellationToken cancellationToken = default)
	{
		var allResourceGroups = await GetAllAsync<ResourceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
		var requestedRootGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroupId) ?? new ResourceGroup { Name = "Root", Id = -1 };
		if (resourceGroupId != requestedRootGroup.Id)
		{
			throw new ArgumentOutOfRangeException($"No ResourceGroup found with id {resourceGroupId}");
		}

		foreach (var resourceGroup in allResourceGroups)
		{
			var parentGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroup.ParentId) ?? (resourceGroupId == -1 ? requestedRootGroup : null);
			if (parentGroup is null)
			{
				continue;
			}

			if (parentGroup.SubGroups is null)
			{
				parentGroup.SubGroups = [resourceGroup];
			}
			else
			{
				parentGroup.SubGroups.Add(resourceGroup);
			}

			var detailedResourceGroup = await GetAsync<ResourceGroup>(
				resourceGroup.Id,
				cancellationToken)
				.ConfigureAwait(false);

			resourceGroup.AlertStatus = detailedResourceGroup.AlertStatus;
		}

		foreach (var resource in await GetAllAsync<Resource>(cancellationToken: cancellationToken).ConfigureAwait(false))
		{
			foreach (var resourceGroup in resource
				.ResourceGroupIdsString
				.Split(',')
				.Select(int.Parse)
				.Select(dgId => allResourceGroups.SingleOrDefault(g => g.Id == dgId)))
			{
				// Avoids a race condition
				if (resourceGroup is null)
				{
					continue;
				}

				resourceGroup.Resources ??= [];
				resourceGroup.Resources.Add(resource);
				resourceGroup.ResourceCount = resourceGroup.Resources.Count;
			}
		}

		return requestedRootGroup;
	}

	/// <summary>
	///     Gets a list of processes being monitored for a Resource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceProcessServiceTaskType">The process/service type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetMonitoredResourceProcessesAsync(
		int resourceId,
		ResourceProcessServiceTaskType deviceProcessServiceTaskType,
		CancellationToken cancellationToken)
	{
		var dataSourceName = deviceProcessServiceTaskType switch
		{
			ResourceProcessServiceTaskType.LinuxProcess => "LinuxNewProcesses-",
			ResourceProcessServiceTaskType.WindowsProcess => "WinProcessStats-",
			ResourceProcessServiceTaskType.WindowsService => "Microsoft_Windows_Services",
			_ => throw new ArgumentException($"Only {ResourceProcessServiceTaskType.LinuxProcess}, {ResourceProcessServiceTaskType.WindowsProcess} and {ResourceProcessServiceTaskType.WindowsService} are supported", nameof(deviceProcessServiceTaskType)),
		};
		var filter = new Filter<ResourceDataSource>
		{
			FilterItems =
				[
					new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceName), dataSourceName),
					new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceType), "DS")
				]
		};
		var deviceDataSources = await GetAllAsync(filter, $"device/devices/{resourceId}/devicedatasources", cancellationToken).ConfigureAwait(false);

		return deviceDataSources.Count != 1
			? ([])
			: await GetAllResourceDataSourceInstancesAsync(
			resourceId,
			deviceDataSources.Single().Id,
			new Filter<ResourceDataSourceInstance>(),
			cancellationToken
		)
		.ConfigureAwait(false);
	}

	/// <summary>
	///     Initiates a task to fetch the list of current running processes on a device
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceProcessServiceTaskType">The device process/service task type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceProcessServiceTask> GetProcessServiceTaskForResourceAsync(
		int resourceId,
		ResourceProcessServiceTaskType resourceProcessServiceTaskType,
		CancellationToken cancellationToken)
		=> PostAsync<ResourceProcessServiceTask, ResourceProcessServiceTask>(
			new ResourceProcessServiceTask
			{
				Type = resourceProcessServiceTaskType
			},
			$"device/devices/{resourceId}/fetchProcessServiceTask",
			cancellationToken
		);

	/// <summary>
	/// For getting process results
	/// </summary>
	[DataContract]
	private class ProcessServiceTaskResultError
	{
		[DataMember(Name = "errorCode")]
		public int ErrorCode { get; set; }

		[DataMember(Name = "errorMessage")]
		public string ErrorMessage { get; set; } = string.Empty;

		[DataMember(Name = "errorDetail")]
		public object ErrorDetail { get; set; } = new();
	}

	/// <summary>
	///     Fetches a task result
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="taskId">The task id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The task results</returns>
	public async Task<Page<ResourceProcess>> GetProcessServiceTaskResultsAsync(
		int resourceId,
		int taskId,
		CancellationToken cancellationToken)
	{
		while (true)
		{
			var jObject = await GetBySubUrlAsync<JObject>($"device/devices/{resourceId}/fetchProcessServiceTask/{taskId}", cancellationToken).ConfigureAwait(false);
			var error = jObject.ToObject<ProcessServiceTaskResultError>();
			if (error is not null && error.ErrorMessage is null && jObject is not null)
			{
				return jObject.ToObject<Page<ResourceProcess>>()
					?? throw new FormatException($"Could not convert response to a page of device process.");
			}

			await Task.Delay(500, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Gets currently-running device processes
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceProcessServiceTaskType">The process/service type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Page<ResourceProcess>> GetResourceProcessesAsync(
		int resourceId,
		ResourceProcessServiceTaskType resourceProcessServiceTaskType,
		CancellationToken cancellationToken)
	{
		var task = await GetProcessServiceTaskForResourceAsync(resourceId, resourceProcessServiceTaskType, cancellationToken).ConfigureAwait(false);
		return await GetProcessServiceTaskResultsAsync(resourceId, task.TaskId, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets a Resource's DataPointConfigurations
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="cancellationToken"></param>
	public Task<List<DataPointConfiguration>> GetResourceDataPointConfigurationsAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync<DataPointConfiguration>(
			$"device/devices/{resourceId}/alertsettings",
			cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire Resource DataSource Instance Group
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The Resource DataSource id</param>
	/// <param name="resourceDataSourceInstanceGroupId">The deviceDataSource Instance Group Id (0 == default)</param>
	/// <param name="dataPointId">The DataPoint Id</param>
	/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
	/// <param name="alertExpressionNote">A note explaining the threshold</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceGroupDataPointThresholdsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceGroupId,
		int dataPointId,
		string alertExpression,
		string alertExpressionNote,
		CancellationToken cancellationToken)
		=> PutAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/{resourceDataSourceInstanceGroupId}/datapoints/{dataPointId}/alertconfig",
			new ThresholdSpecification
			{
				AlertExpression = alertExpression,
				AlertExpressionNote = alertExpressionNote
			}, cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire Resource DataSource Instance Group
	/// </summary>
	/// <param name="resourceGroupId">The Resource id</param>
	/// <param name="dataSourceId">The DataSource id</param>
	/// <param name="dataPointId">The DataPoint Id</param>
	/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
	/// <param name="alertExpressionNote">A note explaining the threshold</param>
	/// <param name="disableAlerting">Disable alerting</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task SetResourceGroupDataSourceDataPointThresholdsAsync(
		int resourceGroupId,
		int dataSourceId,
		int dataPointId,
		string alertExpression,
		string alertExpressionNote,
		bool? disableAlerting,
		CancellationToken cancellationToken)
	{
		// Need to get the existing Alert config, modify it and then PUT it back
		var url = $"device/groups/{resourceGroupId}/datasources/{dataSourceId}/alertsettings";

		var dataPointConfigurationCollection = await GetBySubUrlAsync<DataPointConfigurationCollection>($"device/groups/{resourceGroupId}/datasources/{dataSourceId}/alertsettings", cancellationToken).ConfigureAwait(false);

		var changeMade = false;

		foreach (var dataPointConfiguration in dataPointConfigurationCollection.Items)
		{
			if (dataPointConfiguration.DataPointId == dataPointId)
			{
				if (alertExpression is not null && dataPointConfiguration.AlertExpression != alertExpression)
				{
					dataPointConfiguration.AlertExpression = alertExpression;
					changeMade = true;
				}

				if (alertExpressionNote is not null && dataPointConfiguration.AlertExpressionNote != alertExpressionNote)
				{
					dataPointConfiguration.AlertExpressionNote = alertExpressionNote;
					changeMade = true;
				}

				if (disableAlerting.HasValue && dataPointConfiguration.DisableAlerting != disableAlerting)
				{
					dataPointConfiguration.DisableAlerting = disableAlerting.Value;
					changeMade = true;
				}
			}
		}

		if (changeMade)
		{
			await PutAsync(url, dataPointConfigurationCollection, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Gets a list of DataPointThresholdDetails
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="dataSourceId">The dataSource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DataPointConfiguration>> GetResourceGroupDataPointConfigurationAsync(
		int resourceGroupId,
		int dataSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<DataPointConfigurationCollection>($"device/groups/{resourceGroupId}/datasources/{dataSourceId}/alertsettings", cancellationToken).ConfigureAwait(false)).Items;

	/// <summary>
	/// get Resource instance list
	/// </summary>
	/// <param name="id">The Resource id</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceDataSourceInstance>> GetResourceInstanceListAsync(
		int id,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{id}/instances", filter, cancellationToken);

	/// <summary>
	/// get top talkers graph
	/// </summary>
	public Task<CustomGraphWidgetData> GetTopTalkersGraphAsync(
		int id,
		CancellationToken cancellationToken) => GetBySubUrlAsync<CustomGraphWidgetData>($"device/devices/{id}/topTalkersGraph", cancellationToken);

	/// <summary>
	/// Get Alerts for a Resource by ID
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter">The Alert Filter</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts</returns>
	public async Task<List<Alert>> GetResourceAlertsByIdAsync(
		int resourceId,
		AlertFilter filter,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the device ID set!
		filter.RemoveMonitorObjectReferences();

		var (alerts, limitReached) = await
			GetResourceAlertsByIdNormalAsync(resourceId, filter, false, cancellationToken)
			.ConfigureAwait(false);

		if (limitReached)
		{
			// Fall back to the chunked method
			alerts = await GetResourceAlertsByIdChunkedAsync(
				resourceId,
				filter,
				TimeSpan.FromHours(24),
				cancellationToken)
				.ConfigureAwait(false);
		}

		if (filter?.IsCleared == true)
		{
			return alerts.Where(a => a.IsCleared).ToList();
		}

		return filter?.IsCleared == false ? alerts.Where(a => !a.IsCleared).ToList() : alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter"></param>
	/// <param name="chunkSize">The chunk size (TimeSpan)</param>
	/// <param name="cancellationToken"></param>
	internal async Task<List<Alert>> GetResourceAlertsByIdChunkedAsync(
		int resourceId,
		AlertFilter filter,
		TimeSpan chunkSize,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the device ID set!
		filter.RemoveMonitorObjectReferences();

		var originalStartEpochIsAfter = filter.StartEpochIsAfter;
		var originalStartEpochIsBefore = filter.StartEpochIsBefore;
		var utcNow = DateTime.UtcNow;
		filter.StartEpochIsAfter ??= utcNow.AddYears(-1).SecondsSinceTheEpoch();

		filter.StartEpochIsBefore ??= utcNow.SecondsSinceTheEpoch();

		var allAlerts = new ConcurrentBag<Alert>();

		var alertFilterList = ((long)filter.StartEpochIsAfter).ToDateTimeUtc()
			.GetChunkedTimeRangeList(((long)filter.StartEpochIsBefore).ToDateTimeUtc(), chunkSize)
			.Select(t =>
			{
				var newAlertFilter = filter.Clone();
				newAlertFilter.ResetSearch();
				newAlertFilter.StartEpochIsAfter = t.Item1.SecondsSinceTheEpoch() - 1; // Take one off to include anything raised on that exact second
				newAlertFilter.StartEpochIsBefore = t.Item2.SecondsSinceTheEpoch();
				return newAlertFilter;
			});
		await Task.WhenAll(alertFilterList.Select((Func<AlertFilter, Task>)(async individualAlertFilter =>
		{
			await Task.Delay(_randomGenerator.Next(0, 2000), default).ConfigureAwait(false);
			foreach (var alert in (await GetResourceAlertsByIdNormalAsync((int)resourceId, individualAlertFilter, true, cancellationToken).ConfigureAwait(false)).alerts)
			{
				allAlerts.Add((Alert)alert);
			}
		}))).ConfigureAwait(false);

		filter.StartEpochIsAfter = originalStartEpochIsAfter;
		filter.StartEpochIsBefore = originalStartEpochIsBefore;

		return allAlerts.DistinctBy(a => a.Id).Take(filter.Take ?? int.MaxValue).ToList();
	}

	/// <summary>
	///		Get device alerts with an alert filter (not all properties in the filter are used)
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter">An AlertFilter</param>
	/// <param name="calledFromChunked"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts and whether the limit was reached</returns>
	internal async Task<(List<Alert> alerts, bool limitReached)> GetResourceAlertsByIdNormalAsync(
		int resourceId,
		AlertFilter filter,
		bool calledFromChunked,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the device ID set!
		filter.RemoveMonitorObjectReferences();

		// Ensure skip is set (or 0)
		filter.Skip ??= 0;

		var maxAlertCount = int.MaxValue;
		if (filter.Take is not null)
		{
			if (filter.Take > AlertsMaxTake)
			{
				maxAlertCount = (int)filter.Take;
				filter.Take = AlertsMaxTake;
			}
			else
			{
				maxAlertCount = (int)filter.Take;
			}
		}
		else
		{
			filter.Take = AlertsMaxTake;
		}

		var allAlerts = new List<Alert>();
		do
		{
			var page = await GetBySubUrlAsync<Page<Alert>>($"device/devices/{resourceId}/alerts?{filter.GetFilter()}", cancellationToken).ConfigureAwait(false);

			allAlerts.AddRange(page.Items.Where(alert => !allAlerts.Select(aa => aa.Id).Contains(alert.Id)).ToList());

			if (!calledFromChunked && allAlerts.Count >= 5000)
			{
				// When there are more than 5000 (anywhere near the 10,000 limit), return and use the chunked method instead
				return (new List<Alert>(), true);
			}

			if (page.Items?.Count == 0)
			{
				break;
			}

			if (filter.SearchId is null && !string.IsNullOrWhiteSpace(page.SearchId))
			{
				// We can re-use the searchId
				filter.SearchId = page.SearchId;
			}

			filter.Skip += AlertsMaxTake;
			filter.Take = Math.Min(AlertsMaxTake, maxAlertCount - allAlerts.Count);
		}
		while (filter.Take != 0);

		return (allAlerts, false);
	}

	/// <summary>
	/// Get AWS external id
	/// </summary>
	public Task<AwsExternalId> GetExternalIdAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<AwsExternalId>($"aws/externalId", cancellationToken);

	/// <summary>
	/// Get ResourceGroup SDTs
	/// </summary>
	/// <param name="id"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<ScheduledDownTime>> GetResourceGroupSdtsAsync(
		int id,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/groups/{id}/sdts", filter, cancellationToken);

	/// <summary>
	/// Get ResourceGroup alerts
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="customColumns"></param>
	/// <param name="needMessage"></param>
	/// <param name="fields"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<Alert>> GetResourceGroupAlertsAsync(
		int resourceGroupId,
		string customColumns = "",
		bool needMessage = false,
		string fields = "",
		int size = 50,
		int offset = 0,
		string filter = "",
		CancellationToken cancellationToken = default)
		=> GetBySubUrlAsync<Page<Alert>>($"device/groups/{resourceGroupId}/alerts?customColumns={customColumns}&needMessage={needMessage}&fields={fields}&size={size}&offset={offset}&filter={filter}", cancellationToken);

	/// <summary>
	/// Get unmonitored device list
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<UnmonitoredResource>> GetUnmonitoredResourcesAsync(
		Filter<UnmonitoredResource> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/unmonitoreddevices", filter, cancellationToken);
}
