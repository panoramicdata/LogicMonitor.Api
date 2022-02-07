namespace LogicMonitor.Api.Test.Devices;

public class DataSourceTests : TestWithOutput
{
	public DataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllDeviceDataSourcesAsync()
	{
		var _ = await LogicMonitorClient.GetAllAsync<DataSource>(CancellationToken.None).ConfigureAwait(false);
	}
}
