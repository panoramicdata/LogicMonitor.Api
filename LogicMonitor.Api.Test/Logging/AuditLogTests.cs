namespace LogicMonitor.Api.Test.Logging;

public class AuditLogTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetLogItems_Succeeds()
	{
		var logItems = await LogicMonitorClient
			.GetLogItemsAsync(default)
			.ConfigureAwait(true);

		logItems.Should().NotBeNull();
	}
}
