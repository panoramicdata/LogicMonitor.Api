namespace LogicMonitor.Api.Test.Settings;

public class ExternalAlertsTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<ExternalAlert>(default)
			.ConfigureAwait(true);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetExternalApi()
	{
		var api = await LogicMonitorClient
			.GetExternalApiAsync(default)
			.ConfigureAwait(true);

		api.Should().NotBeNull();
	}
}
