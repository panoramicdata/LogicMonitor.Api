namespace LogicMonitor.Api.Test.Dashboards;

public class DashboardGroupTests : TestWithOutput
{
	private const string DashboardGroupName = "NugetTestDashboardGroup";

	public DashboardGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task CreateDashboardGroup_Succeeds()
	{
		// Delete it if it already exists
		var dashboardGroup = await LogicMonitorClient
			.GetByNameAsync<DashboardGroup>(DashboardGroupName)
			.ConfigureAwait(false);
		if (dashboardGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(dashboardGroup)
				.ConfigureAwait(false);
		}

		// Create it
		dashboardGroup = await LogicMonitorClient
			.CreateAsync(new DashboardGroupCreationDto
			{
				Name = DashboardGroupName,
				Description = "Created by Nuget test",
				CustomProperties = new()
				{
					new Property
					{
						Name = "TestToken",
						Value = "TestValue"
					}
				}
			})
			.ConfigureAwait(false);

		dashboardGroup.Should().NotBeNull();

		dashboardGroup = await LogicMonitorClient
			.GetByNameAsync<DashboardGroup>(DashboardGroupName)
			.ConfigureAwait(false);

		dashboardGroup.Should().NotBeNull();

		dashboardGroup
			.CustomProperties
			.Should()
			.Contain(x => x.Name == "TestToken" && x.Value == "TestValue");
	
		await LogicMonitorClient
			.DeleteAsync(dashboardGroup)
			.ConfigureAwait(false);
	}
}
