// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetResourceGroupEventSources()
	{
		var eventGroupDataSources = await LogicMonitorClient
			.GetAllResourceGroupEventSourcesAsync(1, default)
			.ConfigureAwait(true);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
