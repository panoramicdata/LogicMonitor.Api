namespace LogicMonitor.Api.Test.RecycleBin;

public class RecycleBinTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllRecycleBinItems()
	{
		var recycleBinItems = await LogicMonitorClient.GetAllAsync<RecycleBinItem>(default).ConfigureAwait(true);
		recycleBinItems.Should().NotBeNull();
	}
}
