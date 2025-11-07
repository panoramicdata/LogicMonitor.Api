namespace LogicMonitor.Api.Test.Settings;

public class ExternalAlertsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllAsync()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<ExternalAlert>(CancellationToken);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetExternalApiAsync()
	{
		var api = await LogicMonitorClient
			.GetExternalApiAsync(CancellationToken);

		api.Should().NotBeNull();
	}
}
