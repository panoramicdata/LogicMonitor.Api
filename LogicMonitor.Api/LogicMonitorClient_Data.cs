namespace LogicMonitor.Api;

/// <summary>
///     Data Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets ForecastData
	/// </summary>
	/// <param name="forecastDataRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ForecastGraphData> GetForecastGraphDataAsync(
		ForecastDataRequest forecastDataRequest,
		CancellationToken cancellationToken)
	{
		var trainingGraphData = await GetBySubUrlAsync<GraphData>(forecastDataRequest.TrainingSubUrl, cancellationToken).ConfigureAwait(false);

		var forcastedGraphData = await GetBySubUrlAsync<GraphData>(forecastDataRequest.ForecastedSubUrl, cancellationToken).ConfigureAwait(false);

		return new ForecastGraphData
		{
			// graphs/trainingdata?_scope=internal&graphId=3040&dataSourceInstanceId=9053&type=host&selectedDataPointLabel=SIZEGB&maxSamples=249&time=1month&startTime=&endTime=&_=1481203870059
			TrainingGraphData = trainingGraphData,
			// device/devicedatasourceinstances/9065/graphs/3043/data/forecasting?_scope=internal&selectedDataPointLabel=CPU&maxSamples=286&end=1487982545&start=1485390545&time=1month&forecastTime=7days&_=1487979440482
			ForecastedGraphData = forcastedGraphData
		};
	}

	/// <summary>
	///     Get graph data using the REST API
	/// </summary>
	/// <param name="graphDataRequest">The </param>
	/// <param name="cancellationToken">Optional cancellation token</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<GraphData> GetGraphDataAsync(GraphDataRequest graphDataRequest, CancellationToken cancellationToken)
	{
		if (graphDataRequest is null)
		{
			throw new ArgumentNullException(nameof(graphDataRequest));
		}

		graphDataRequest.Validate();

		var subUrl = graphDataRequest.SubUrl;
		var graphData = await GetBySubUrlAsync<GraphData>(subUrl, cancellationToken).ConfigureAwait(false);

		// RM-14879 Fix duplicate datapoint values due to DEVTS-14598
		graphData.RemoveInvalidDataPoints();
		return graphData;
	}

	/// <summary>
	///     Get overview graphs
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DataSourceGraph>> GetDeviceOverviewGraphsAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
	{
		var deviceDataSources = (await GetBySubUrlAsync<Page<DeviceDataSource>>($"device/devices/{resourceId}/devicedatasources", cancellationToken).ConfigureAwait(false)).Items;
		var filteredDeviceDataSource = deviceDataSources.SingleOrDefault(dds => dds.Id == resourceDataSourceId)
			?? throw new ArgumentException($"No datasource on device {resourceId} with deviceDataSourceId {resourceDataSourceId}.",
				nameof(resourceDataSourceId));
		return filteredDeviceDataSource.OverviewGraphs;
	}

	/// <summary>
	///     Gets a Device OverviewGraph By Name
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The device dataSource Id</param>
	/// <param name="name">The overview graph name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DataSourceGraph> GetDeviceOverviewGraphByNameAsync(
		int resourceId,
		int resourceDataSourceId,
		string name,
		CancellationToken cancellationToken)
		=> (await GetDeviceOverviewGraphsAsync(resourceId, resourceDataSourceId, cancellationToken).ConfigureAwait(false))
			.SingleOrDefault(g => g.Name == name);

	/// <summary>
	///     Get Raw data (the last measurements, up to a maximum of ten)
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The ResourceDataSource instance id</param>
	/// <param name="startDateTimeUtc"></param>
	/// <param name="endDateTimeUtc"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<RawDataSet> GetRawDataSetAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		DateTime? startDateTimeUtc,
		DateTime? endDateTimeUtc,
		CancellationToken cancellationToken
		)
	{
		var timeConstraint = startDateTimeUtc.HasValue && endDateTimeUtc.HasValue ? $"?start={startDateTimeUtc.Value.SecondsSinceTheEpoch()}&end={endDateTimeUtc.Value.SecondsSinceTheEpoch()}" : null;
		return GetAsync<RawDataSet>(false, $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/data{timeConstraint}", cancellationToken);
	}

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The Resource datasource id</param>
	/// <param name="resourceDataSourceInstanceId">The Resource datasource instance id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	/// <returns>An object with information on what to poll for to get results</returns>
	public Task<PollNowRequest> GetPollNowRequestAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> PostAsync<EmptyRequest, PollNowRequest>(new EmptyRequest(), $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/data/pollnow", cancellationToken);

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The device datasource id</param>
	/// <param name="resourceDataSourceInstanceId">The device datasource instance id</param>
	/// <param name="requestId">The request id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	public Task<PollNowResponse> GetPollNowResponseAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		long requestId,
		CancellationToken cancellationToken)
		=> GetAsync<PollNowResponse>(false, $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/data/pollnow/{requestId}", cancellationToken);

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The device datasource id</param>
	/// <param name="resourceDataSourceInstanceId">The device datasource instance id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	public async Task<PollNowResponse> PollNowAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
	{
		var pollNowRequest = await GetPollNowRequestAsync(resourceId, resourceDataSourceId, resourceDataSourceInstanceId, cancellationToken).ConfigureAwait(false);

		while (true)
		{
			var pollNowResponse = await GetPollNowResponseAsync(resourceId, resourceDataSourceId, resourceDataSourceInstanceId, pollNowRequest.RequestId, cancellationToken).ConfigureAwait(false);

			if (pollNowResponse.RequestStatus == PollNowStatus.Done)
			{
				return pollNowResponse;
			}

			// Wait for 1 second before trying again
			await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Fetch data across multiple instances
	/// </summary>
	/// <param name="resourceDataSourceInstanceIds">The list of DeviceSataSourceInstance ids</param>
	/// <param name="start">The start</param>
	/// <param name="end">The start</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	/// <returns>The FetchDataResponse</returns>
	public async Task<FetchDataResponse> GetFetchDataResponseAsync(
		List<int> resourceDataSourceInstanceIds,
		DateTimeOffset start,
		DateTimeOffset end,
		CancellationToken cancellationToken)
	{
		var startDateTimeUtcTimestamp = start.ToUnixTimeSeconds();
		var endDateTimeUtcTimestamp = end.ToUnixTimeSeconds();
		return await PostAsync<FetchDataRequest, FetchDataResponse>(
			new FetchDataRequest { InstanceIds = string.Join(",", resourceDataSourceInstanceIds) },
			$"device/instances/datafetch?start={startDateTimeUtcTimestamp}&end={endDateTimeUtcTimestamp}",
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Get Resource DataSource data
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	public Task<DeviceDataSourceData> GetDeviceDataSourceDataAsync(
		int resourceId,
		int id,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DeviceDataSourceData>($"device/devices/{resourceId}/devicedatasources/{id}/data", cancellationToken);
}
