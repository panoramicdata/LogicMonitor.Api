namespace LogicMonitor.Api.Test.Settings;

public class ExternalAlertsTests : TestWithOutput
{
	public ExternalAlertsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var items = await LogicMonitorClient.GetAllAsync<ExternalAlert>().ConfigureAwait(false);
		Assert.NotNull(items);
		Assert.True(items.Count > 0);
	}
}
