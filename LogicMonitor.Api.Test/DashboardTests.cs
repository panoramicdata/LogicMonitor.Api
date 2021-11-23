using LogicMonitor.Api.Dashboards;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test;

public class DashboardTests : TestWithOutput
{
	public DashboardTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetDashboardsNoWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>().ConfigureAwait(false);

		// Make sure that some are returned
		Assert.True(dashboards.Count > 0);

		// Make sure that all have Unique Ids
		Assert.False(dashboards.Select(c => c.Id).HasDuplicates());
	}

	[Fact]
	public async void GetDashboardsWithWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>().ConfigureAwait(false);

		// Make sure that some are returned
		Assert.True(dashboards.Count > 0);

		// Make sure that all have Unique Ids
		Assert.False(dashboards.Select(c => c.Id).HasDuplicates());
	}
}
