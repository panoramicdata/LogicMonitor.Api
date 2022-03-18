namespace LogicMonitor.Api.Test.RecycleBin;

public class RecycleBinTests : TestWithOutput
{
	public RecycleBinTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllRecycleBinItems()
	{
		var recycleBinItems = await LogicMonitorClient.GetAllAsync<RecycleBinItem>().ConfigureAwait(false);
		recycleBinItems.Should().NotBeNull();
	}
}
