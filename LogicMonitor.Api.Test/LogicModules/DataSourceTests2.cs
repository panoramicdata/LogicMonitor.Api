namespace LogicMonitor.Api.Test.Devices;

public class DataSourceTests2 : TestWithOutput
{
	public DataSourceTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var _ = await LogicMonitorClient.GetAllAsync<DataSource>(CancellationToken.None).ConfigureAwait(false);
	}
}
