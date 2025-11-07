namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests2(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var dataSources = await LogicMonitorClient
			.GetAllAsync<DataSource>(CancellationToken);
		dataSources.Should().NotBeNullOrEmpty();
	}
}
