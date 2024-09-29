namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests2(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var dataSources = await LogicMonitorClient
			.GetAllAsync<DataSource>(default)
			.ConfigureAwait(true);
		dataSources.Should().NotBeNullOrEmpty();
	}
}
