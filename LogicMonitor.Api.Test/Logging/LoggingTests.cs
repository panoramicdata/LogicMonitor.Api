namespace LogicMonitor.Api.Test.Logging;

public class PushMetricTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task WriteLogAsync_Succeeds()
	{
		var response = await LogicMonitorClient
			.WriteLogAsync(WindowsDeviceId, "Test log message.", default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task GetLogItems_Succeeds()
	{
		var logItems = await LogicMonitorClient
			.GetLogItemsAsync(default)
			.ConfigureAwait(true);

		logItems.Should().NotBeNull();
	}
}
