namespace LogicMonitor.Api.Test.Settings;

public class ExternalAlertsTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllAsync()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<ExternalAlert>(default)
			.ConfigureAwait(true);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetExternalApiAsync()
	{
		var api = await LogicMonitorClient
			.GetExternalApiAsync(default)
			.ConfigureAwait(true);

		api.Should().NotBeNull();
	}
}
