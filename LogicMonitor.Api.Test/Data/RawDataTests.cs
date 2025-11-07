namespace LogicMonitor.Api.Test.Data;

public class RawDataTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetRawData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), CancellationToken);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, null, null, CancellationToken);

		rawData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWebsiteCheckpointRawData()
	{
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, CancellationToken);

		website.Should().NotBeNull();

		foreach (var checkpoint in website.Checkpoints ?? [])
		{
			var rawData =
				await LogicMonitorClient
				.GetWebsiteCheckpointRawDataSet(
					new WebsiteCheckPointRawDataRequest
					{
						WebsiteCheckPointId = checkpoint.Id,
						WebsiteId = website.Id,
					},
					CancellationToken);

			rawData.Should().NotBeNull();
			rawData.DataPoints.Should().NotBeEmpty();
			rawData.UtcTimeStamps.Should().NotBeEmpty();
		}

		foreach (var checkpoint in website.Checkpoints ?? [])
		{
			var rawData =
				await LogicMonitorClient
				.GetWebsiteCheckpointRawDataSet(
					new WebsiteCheckPointRawDataRequest
					{
						DataPointNames = ["sslDaysUntilExpiration"],
						WebsiteCheckPointId = checkpoint.Id,
						WebsiteId = website.Id,
					},
					CancellationToken);

			rawData.Should().NotBeNull();
			rawData.DataPoints.Should().NotBeEmpty();
			rawData.DataPoints.Should().ContainSingle();
			rawData.UtcTimeStamps.Should().NotBeEmpty();

			var rawData2 =
				await LogicMonitorClient
				.GetWebsiteCheckpointRawDataSet(
					new WebsiteCheckPointRawDataRequest
					{
						DataPointNames = ["sslDaysUntilExpiration"],
						TimePeriod = WebsiteCheckpointRawDataTimePeriod.TwoHours,
						WebsiteCheckPointId = checkpoint.Id,
						WebsiteId = website.Id,
					},
					CancellationToken);

			rawData2.Should().NotBeNull();
			rawData2.DataPoints.Should().NotBeEmpty();
			rawData2.DataPoints.Should().ContainSingle();
			rawData2.UtcTimeStamps.Should().HaveCountGreaterThan(rawData.UtcTimeStamps.Count);
			rawData2.UtcTimeStamps.Should().NotBeEmpty();

			// Check there is only 1 timestamp / value for the aggregation type First
			var rawData3 =
				await LogicMonitorClient
				.GetWebsiteCheckpointRawDataSet(
					new WebsiteCheckPointRawDataRequest
					{
						Aggregation = WebsiteCheckpointRawDataAggregationType.First,
						TimePeriod = WebsiteCheckpointRawDataTimePeriod.TwoHours,
						WebsiteCheckPointId = checkpoint.Id,
						WebsiteId = website.Id,
					},
					CancellationToken);

			rawData3.Should().NotBeNull();
			rawData3.DataPoints.Should().NotBeEmpty();
			rawData3.UtcTimeStamps.Should().ContainSingle();
			rawData3.Values.Should().ContainSingle();
			rawData3.ValuesAsObjects.Should().ContainSingle();
		}
	}

	[Fact]
	public async Task GetRawDataTimeConstrained()
	{
		var utcNow = DateTime.UtcNow;
		var yesterday = utcNow - TimeSpan.FromDays(1);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), CancellationToken);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow, CancellationToken);

		rawData.Should().NotBeNull();

		rawData.UtcTimeStamps.Should().AllSatisfy(r =>
		{
			var dataDateTime = r.ToDateTimeUtcFromMs();
			(yesterday <= dataDateTime).Should().BeTrue();
			(dataDateTime <= utcNow).Should().BeTrue();
		});
	}

	[Fact]
	public async Task PollNow()
	{
		var portalClient = LogicMonitorClient;
		var dataSource = await portalClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await portalClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstances = await portalClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), CancellationToken);
		var deviceDataSourceInstance = deviceDataSourceInstances.FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var pollNowResponse = await portalClient
			.PollNowAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance!.Id, CancellationToken);

		pollNowResponse.Should().NotBeNull();
	}

	[Fact]
	public async Task FetchInstanceData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
				WindowsDeviceId,
				dataSource.Id,
				CancellationToken);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(
				WindowsDeviceId,
				deviceDataSource.Id,
				new(),
				CancellationToken);
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var end = DateTime.UtcNow;
		var start = end.AddHours(-2);

		var rawData = await LogicMonitorClient
			.GetFetchDataResponseAsync(deviceDataSourceInstances.ConvertAll(ddsi => ddsi.Id), start, end, CancellationToken);

		rawData.Should().NotBeNull();
		rawData.TotalCount.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses.Should().HaveCount(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses
			.Should()
			.AllSatisfy(response => response.DataPoints.Count.Should().Be(dataSource.DataSourceDataPoints.Count));
	}
}
