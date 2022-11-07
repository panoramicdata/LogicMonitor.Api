namespace LogicMonitor.Api.Test.Logging;

public class PushMetricTests : TestWithOutput
{
	public PushMetricTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task WriteLogAsync_Succeeds()
	{
		var response = await LogicMonitorClient
			.WriteLogAsync(WindowsDeviceId, "Test log message.")
			.ConfigureAwait(false);
		response.Should().NotBeNull();
	}
}
