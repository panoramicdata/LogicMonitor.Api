namespace LogicMonitor.Api.Test.Data;

public class RawDataTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetRawData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource!.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(425, deviceDataSource.Id, deviceDataSourceInstance.Id, null, null, cancellationToken: default)
			.ConfigureAwait(true);

		rawData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRawDataTimeConstrained()
	{
		var utcNow = DateTime.UtcNow;
		var yesterday = utcNow - TimeSpan.FromDays(1);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource!.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.Single();
		var rawData = await LogicMonitorClient
			.GetRawDataSetAsync(425, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow, default)
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
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await portalClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource!.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstances = await portalClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances.FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var pollNowResponse = await portalClient
			.PollNowAsync(425, deviceDataSource.Id, deviceDataSourceInstance!.Id, default)
			.ConfigureAwait(true);

		pollNowResponse.Should().NotBeNull();
	}

	[Fact]
	public async Task FetchInstanceData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource!.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);

		var end = DateTime.UtcNow;
		var start = end.AddHours(-2);

		var rawData = await LogicMonitorClient
			.GetFetchDataResponseAsync(deviceDataSourceInstances.ConvertAll(ddsi => ddsi.Id), start, end, cancellationToken: default)
			.ConfigureAwait(true);

		rawData.Should().NotBeNull();
		rawData.TotalCount.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses.Count.Should().Be(deviceDataSourceInstances.Count);
		rawData.InstanceFetchDataResponses
			.Should()
			.AllSatisfy(response => response.DataPoints.Count.Should().Be(dataSource.DataSourceDataPoints.Count));
	}
}
