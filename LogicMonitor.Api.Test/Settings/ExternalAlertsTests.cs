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
			.GetAllAsync<ExternalAlert>(CancellationToken.None)
			.ConfigureAwait(false);
		items.Should().NotBeNull();
	}
}
