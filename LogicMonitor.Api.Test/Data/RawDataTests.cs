namespace LogicMonitor.Api.Test.Data;

public class RawDataTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetRawData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, null, null, cancellationToken: default)
			.ConfigureAwait(true);

		rawData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRawDataTimeConstrained()
	{
		var utcNow = DateTime.UtcNow;
		var yesterday = utcNow - TimeSpan.FromDays(1);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow, default)
			.ConfigureAwait(true);

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
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await portalClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstances = await portalClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var pollNowResponse = await portalClient
			.PollNowAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance!.Id, default)
			.ConfigureAwait(true);

		pollNowResponse.Should().NotBeNull();
	}

	[Fact]
	public async Task FetchInstanceData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
				WindowsDeviceId,
				dataSource.Id,
				default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(
				WindowsDeviceId,
				deviceDataSource.Id,
				new(),
				cancellationToken: default)
			.ConfigureAwait(true);
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var end = DateTime.UtcNow;
		var start = end.AddHours(-2);

		var rawData = await LogicMonitorClient
			.GetFetchDataResponseAsync(deviceDataSourceInstances.ConvertAll(ddsi => ddsi.Id), start, end, cancellationToken: default)
			.ConfigureAwait(true);

		rawData.Should().NotBeNull();
		rawData.TotalCount.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses.Should().HaveCount(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses
			.Should()
			.AllSatisfy(response => response.DataPoints.Count.Should().Be(dataSource.DataSourceDataPoints.Count));
	}
}
