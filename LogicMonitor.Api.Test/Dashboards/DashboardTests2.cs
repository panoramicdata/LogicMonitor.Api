using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Dashboards;

public class DashboardTests2 : TestWithOutput
{
	public DashboardTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetDashboardsNoWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>().ConfigureAwait(false);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async void GetDashboardsWithWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>().ConfigureAwait(false);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}
}
