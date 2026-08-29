namespace LogicMonitor.Api.Test.RecycleBin;

public class RecycleBinTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllRecycleBinItems()
	{
		var recycleBinItems = await LogicMonitorClient.GetAllAsync<RecycleBinItem>(CancellationToken);
		recycleBinItems.Should().NotBeNull();
	}
}
