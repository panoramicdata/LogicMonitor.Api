namespace LogicMonitor.Api.Test.Devices;

public class DataSourceTests2(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var dataSources = await LogicMonitorClient
			.GetDatasourceListAsync(new())
			.ConfigureAwait(true);
		dataSources.Items.Should().NotBeNullOrEmpty();
	}
}
