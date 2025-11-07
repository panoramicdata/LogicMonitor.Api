namespace LogicMonitor.Api.Test.Resources;

public class UnmonitoredResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetUnmonitoredResources()
	{
		var unmonitoredResources = await LogicMonitorClient
			.GetUnmonitoredResourcesAsync(new Filter<UnmonitoredResource>(), CancellationToken);
		unmonitoredResources.Should().NotBeNull();
	}
}
