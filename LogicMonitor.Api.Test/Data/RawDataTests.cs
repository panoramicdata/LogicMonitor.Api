namespace LogicMonitor.Api.Test.Data;

public class RawDataTests : TestWithOutput
{
	public RawDataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetRawData()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id).ConfigureAwait(false);
		var deviceDataSourceInstance =
		(await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
		).Single();
		var rawData = await LogicMonitorClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);

		rawData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRawDataTimeConstrained()
	{
		var utcNow = DateTime.UtcNow;
		var yesterday = utcNow - TimeSpan.FromDays(1);
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id).ConfigureAwait(false);
		var deviceDataSourceInstance =
		(await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
		).Single();
		var rawData = await LogicMonitorClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow).ConfigureAwait(false);

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
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var dataSource = await portalClient.GetDataSourceByUniqueNameAsync("WinIf-").ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id).ConfigureAwait(false);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstance =
		(await portalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
		).FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var pollNowResponse = await portalClient.PollNowAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance!.Id).ConfigureAwait(false);

		pollNowResponse.Should().NotBeNull();
	}

	[Fact]
	public async Task FetchInstanceData()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinIf-").ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id).ConfigureAwait(false);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);

		var end = DateTime.UtcNow;
		var start = end.AddHours(-2);

		var rawData = await LogicMonitorClient.GetFetchDataResponseAsync(deviceDataSourceInstances.ConvertAll(ddsi => ddsi.Id), start, end).ConfigureAwait(false);

		rawData.Should().NotBeNull();
		rawData.TotalCount.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses.Count.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses
			.Should()
			.AllSatisfy(response => response.DataPoints.Length.Should().Be(dataSource.DataSourceDataPoints.Count));
	}
}
