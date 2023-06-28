namespace LogicMonitor.Api.Test.Settings;

public class ExternalAlertsTests : TestWithOutput
{
	public ExternalAlertsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<ExternalAlert>(default)
			.ConfigureAwait(false);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetExternalApi()
	{
		var api = await LogicMonitorClient
			.GetExternalApiAsync(default)
			.ConfigureAwait(false);

		api.Should().NotBeNull();
	}
}
