// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetDeviceGroupEventSources()
	{
		var eventGroupDataSources = await LogicMonitorClient
			.GetAllDeviceGroupEventSourcesAsync(1, default)
			.ConfigureAwait(true);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
