namespace LogicMonitor.Api.Test.RecycleBin;

public class RecycleBinTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllRecycleBinItems()
	{
		var recycleBinItems = await LogicMonitorClient.GetAllAsync<RecycleBinItem>(default).ConfigureAwait(true);
		recycleBinItems.Should().NotBeNull();

		// The following bit of code is for testing restores
		if (recycleBinItems.Count > 0 && false)
		{
			var response = await LogicMonitorClient.RecycleBinRestoreAsync([recycleBinItems[0].Id], CancellationToken.None);
		}
	}
}
