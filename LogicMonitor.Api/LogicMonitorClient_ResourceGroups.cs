namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the full Resource tree
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroup> GetFullResourceTreeAsync(
		[Optional, DefaultParameterValue(-1)] int resourceGroupId,
		[Optional] CancellationToken cancellationToken)
	{
		var allResourceGroups = await GetAllAsync<ResourceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
		var requestedRootGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroupId) ?? new ResourceGroup { Name = "Root", Id = -1 };
		if (resourceGroupId != requestedRootGroup.Id)
		{
			throw new ArgumentOutOfRangeException($"No ResourceGroup found with id {resourceGroupId}");
		}

		await LinkResourceGroupsAsync(allResourceGroups, requestedRootGroup, resourceGroupId, cancellationToken).ConfigureAwait(false);

		AttachResources(allResourceGroups, await GetAllAsync<Resource>(cancellationToken: cancellationToken).ConfigureAwait(false));

		return requestedRootGroup;
	}

	private async Task LinkResourceGroupsAsync(
		List<ResourceGroup> allResourceGroups,
		ResourceGroup requestedRootGroup,
		int resourceGroupId,
		CancellationToken cancellationToken)
	{
		foreach (var resourceGroup in allResourceGroups)
		{
			var parentGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroup.ParentId) ?? (resourceGroupId == -1 ? requestedRootGroup : null);
			if (parentGroup is null)
			{
				continue;
			}

			parentGroup.SubGroups ??= [];
			parentGroup.SubGroups.Add(resourceGroup);

			var detailedResourceGroup = await GetAsync<ResourceGroup>(
				resourceGroup.Id,
				cancellationToken)
				.ConfigureAwait(false);

			resourceGroup.AlertStatus = detailedResourceGroup.AlertStatus;
		}
	}

	private static void AttachResources(List<ResourceGroup> allResourceGroups, List<Resource> resources)
	{
		foreach (var resource in resources)
		{
			var resourceGroups = resource
				.ResourceGroupIdsString
				.Split(',')
				.Select(int.Parse)
				.Select(dgId => allResourceGroups.SingleOrDefault(g => g.Id == dgId))
				.OfType<ResourceGroup>();

			// A null group is skipped, which avoids a race condition
			foreach (var resourceGroup in resourceGroups)
			{
				resourceGroup.Resources ??= [];
				resourceGroup.Resources.Add(resource);
				resourceGroup.ResourceCount = resourceGroup.Resources.Count;
			}
		}
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

	private const int ProcessServiceTaskRunningErrorCode = 1202;

	private static readonly TimeSpan DefaultProcessServiceTaskMaximumWait = TimeSpan.FromMinutes(5);

	/// <summary>
	///     Fetches a task result, waiting up to five minutes for the collector to finish
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="taskId">The task id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The task results</returns>
	public Task<Page<ResourceProcess>> GetProcessServiceTaskResultsAsync(
		int resourceId,
		int taskId,
		CancellationToken cancellationToken)
		=> GetProcessServiceTaskResultsAsync(resourceId, taskId, DefaultProcessServiceTaskMaximumWait, cancellationToken);

	/// <summary>
	///     Fetches a task result, waiting up to maximumWait for the collector to finish
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="taskId">The task id</param>
	/// <param name="maximumWait">How long to wait for the task before throwing a TimeoutException</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The task results</returns>
	public async Task<Page<ResourceProcess>> GetProcessServiceTaskResultsAsync(
		int resourceId,
		int taskId,
		TimeSpan maximumWait,
		CancellationToken cancellationToken)
	{
		var stopwatch = Stopwatch.StartNew();
		while (true)
		{
			// Uncached: each poll must reach the portal, and a finished result can only be read once
			var response = await GetAsync<JObject>(false, $"device/devices/{resourceId}/fetchProcessServiceTask/{taskId}", cancellationToken).ConfigureAwait(false);
			var page = ReadProcessServiceTaskResult(response);
			if (page is not null)
			{
				return page;
			}

			if (stopwatch.Elapsed >= maximumWait)
			{
				throw new TimeoutException($"Process/service task {taskId} on Resource {resourceId} was still running after {maximumWait}.");
			}

			await Task.Delay(500, cancellationToken).ConfigureAwait(false);
		}
	}
}
