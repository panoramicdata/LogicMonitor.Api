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
		CancellationToken cancellationToken) => new ForecastGraphData
		{
			// graphs/trainingdata?_scope=internal&graphId=3040&dataSourceInstanceId=9053&type=host&selectedDataPointLabel=SIZEGB&maxSamples=249&time=1month&startTime=&endTime=&_=1481203870059
			TrainingGraphData = await GetBySubUrlAsync<GraphData>(forecastDataRequest.TrainingSubUrl, cancellationToken).ConfigureAwait(false),
			// device/devicedatasourceinstances/9065/graphs/3043/data/forecasting?_scope=internal&selectedDataPointLabel=CPU&maxSamples=286&end=1487982545&start=1485390545&time=1month&forecastTime=7days&_=1487979440482
			ForecastedGraphData = await GetBySubUrlAsync<GraphData>(forecastDataRequest.ForecastedSubUrl, cancellationToken).ConfigureAwait(false)
		};

	/// <summary>
	///     Get graph data using the REST API
	/// </summary>
	/// <param name="graphDataRequest">The </param>
	/// <param name="cancellationToken">Optional cancellation token</param>
	/// <exception cref="ArgumentNullException"></exception>
	public Task<GraphData> GetGraphDataAsync(GraphDataRequest graphDataRequest, CancellationToken cancellationToken)
	{
		if (graphDataRequest is null)
		{
			throw new ArgumentNullException(nameof(graphDataRequest));
		}

		graphDataRequest.Validate();

		var subUrl = graphDataRequest.SubUrl;
		return GetBySubUrlAsync<GraphData>(subUrl, cancellationToken);
	}

	/// <summary>
	///     Get overview graphs
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DataSourceOverviewGraph>> GetDeviceOverviewGraphsAsync(int deviceId, int deviceDataSourceId, CancellationToken cancellationToken)
	{
		// return Get<OverviewGraphCollection>(ApiMethod.Do, $"ograph?func=getGroups&hId={deviceId}&dsId={dataSourceId}&dsigId={dataSourceInstanceGroupId}");
		// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/575/devicedatasources
		var deviceDataSources = (await GetBySubUrlAsync<Page<DeviceDataSource>>($"device/devices/{deviceId}/devicedatasources", cancellationToken).ConfigureAwait(false)).Items;
		var filteredDeviceDataSource = deviceDataSources.SingleOrDefault(dds => dds.Id == deviceDataSourceId)
			?? throw new ArgumentException($"No datasource on device {deviceId} with deviceDataSourceId {deviceDataSourceId}.",
				nameof(deviceDataSourceId));
		return filteredDeviceDataSource.OverviewGraphs;
	}

	/// <summary>
	///     Gets a Device OverviewGraph By Name
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device dataSource Id</param>
	/// <param name="name">The overview graph name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DataSourceOverviewGraph> GetDeviceOverviewGraphByNameAsync(int deviceId, int deviceDataSourceId, string name, CancellationToken cancellationToken)
		=> (await GetDeviceOverviewGraphsAsync(deviceId, deviceDataSourceId, cancellationToken).ConfigureAwait(false))
			.SingleOrDefault(g => g.Name == name);

	/// <summary>
	///     Get Raw data (the last measurements, up to a maximum of ten)
	/// </summary>
	/// <param name="deviceId">The device id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="deviceDataSourceInstanceId">The device data source instance id</param>
	/// <param name="startDateTimeUtc"></param>
	/// <param name="endDateTimeUtc"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<RawDataSet> GetRawDataSetAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		DateTime? startDateTimeUtc,
		DateTime? endDateTimeUtc,
		CancellationToken cancellationToken
		)
	{
		var timeConstraint = startDateTimeUtc.HasValue && endDateTimeUtc.HasValue ? $"?start={startDateTimeUtc.Value.SecondsSinceTheEpoch()}&end={endDateTimeUtc.Value.SecondsSinceTheEpoch()}" : null;
		return GetAsync<RawDataSet>(false, $"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/data{timeConstraint}", cancellationToken);
	}

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="deviceId">The device id</param>
	/// <param name="deviceDataSourceId">The device datasource id</param>
	/// <param name="deviceDataSourceInstanceId">The device datasource instance id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	/// <returns>An object with information on what to poll for to get results</returns>
	public Task<PollNowRequest> GetPollNowRequestAsync(int deviceId, int deviceDataSourceId, int deviceDataSourceInstanceId, CancellationToken cancellationToken)
		=> PostAsync<EmptyRequest, PollNowRequest>(new EmptyRequest(), $"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/data/pollnow", cancellationToken);

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="deviceId">The device id</param>
	/// <param name="deviceDataSourceId">The device datasource id</param>
	/// <param name="deviceDataSourceInstanceId">The device datasource instance id</param>
	/// <param name="requestId">The request id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	public Task<PollNowResponse> GetPollNowResponseAsync(int deviceId, int deviceDataSourceId, int deviceDataSourceInstanceId, long requestId, CancellationToken cancellationToken)
		=> GetAsync<PollNowResponse>(false, $"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/data/pollnow/{requestId}", cancellationToken);

	/// <summary>
	/// Polls now for data via the collector
	/// </summary>
	/// <param name="deviceId">The device id</param>
	/// <param name="deviceDataSourceId">The device datasource id</param>
	/// <param name="deviceDataSourceInstanceId">The device datasource instance id</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	public async Task<PollNowResponse> PollNowAsync(int deviceId, int deviceDataSourceId, int deviceDataSourceInstanceId, CancellationToken cancellationToken)
	{
		var pollNowRequest = await GetPollNowRequestAsync(deviceId, deviceDataSourceId, deviceDataSourceInstanceId, cancellationToken).ConfigureAwait(false);

		while (true)
		{
			var pollNowResponse = await GetPollNowResponseAsync(deviceId, deviceDataSourceId, deviceDataSourceInstanceId, pollNowRequest.RequestId, cancellationToken).ConfigureAwait(false);

			if (pollNowResponse.RequestStatus == PollNowStatus.Done)
			{
				return pollNowResponse;
			}

			// Wait for 1 second before trying again
			await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Fetch data across mutliple instances
	/// </summary>
	/// <param name="deviceDataSourceInstanceIds">The list of DeviceSataSourceInstance ids</param>
	/// <param name="start">The start</param>
	/// <param name="end">The start</param>
	/// <param name="cancellationToken">The optional CancellationToken</param>
	/// <returns>The FetchDataResponse</returns>
	public async Task<FetchDataResponse> GetFetchDataResponseAsync(
		List<int> deviceDataSourceInstanceIds,
		DateTimeOffset start,
		DateTimeOffset end,
		CancellationToken cancellationToken)
	{
		var startDateTimeUtcTimestamp = start.ToUnixTimeSeconds();
		var endDateTimeUtcTimestamp = end.ToUnixTimeSeconds();
		return await PostAsync<FetchDataRequest, FetchDataResponse>(
			new FetchDataRequest { InstanceIds = string.Join(",", deviceDataSourceInstanceIds) },
			$"device/instances/datafetch?start={startDateTimeUtcTimestamp}&end={endDateTimeUtcTimestamp}",
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Get device datasource data
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	public async Task<DeviceDataSourceData> GetDeviceDataSourceDataAsync(
		int deviceId,
		int id,
		CancellationToken cancellationToken)
		=> await GetBySubUrlAsync<DeviceDataSourceData>($"device/devices/{deviceId}/devicedatasources{id}/data", cancellationToken);
}
