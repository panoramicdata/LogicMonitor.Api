using LogicMonitor.Api.Resources;

namespace LogicMonitor.Api.Test.Resources;

public class UnmonitoredResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetUnmonitoredResources()
	{
		var unmonitoredResources = await LogicMonitorClient
			.GetUnmonitoredResourcesAsync(new Filter<UnmonitoredResource>(), default)
			.ConfigureAwait(true);
		unmonitoredResources.Should().NotBeNull();
	}
}
