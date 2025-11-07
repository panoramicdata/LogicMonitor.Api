// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetResourceGroupEventSources()
	{
		var eventGroupDataSources = await LogicMonitorClient
			.GetAllResourceGroupEventSourcesAsync(1, CancellationToken);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
