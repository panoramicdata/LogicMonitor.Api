namespace LogicMonitor.Api.Test.RecycleBin;

public class RecycleBinTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllRecycleBinItems()
	{
		var recycleBinItems = await LogicMonitorClient.GetAllAsync<RecycleBinItem>(default).ConfigureAwait(true);
		recycleBinItems.Should().NotBeNull();
	}
}
