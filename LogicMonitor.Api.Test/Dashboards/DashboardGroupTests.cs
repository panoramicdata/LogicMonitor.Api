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
			.GetByNameAsync<DashboardGroup>(DashboardGroupName, default)
			.ConfigureAwait(false);
		if (dashboardGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(dashboardGroup, default)
				.ConfigureAwait(false);
		}

		// Create it
		dashboardGroup = await LogicMonitorClient
			.CreateAsync(new DashboardGroupCreationDto
			{
				ParentId = "1",
				Name = DashboardGroupName,
				Description = "Created by Nuget test",
				CustomProperties = new()
				{
					new SimpleProperty
					{
						Name = "TestToken",
						Value = "TestValue",
						Type = SimplePropertyType.Owned
					}
				}
			},
			default)
			.ConfigureAwait(false);

		dashboardGroup.Should().NotBeNull();

		dashboardGroup = await LogicMonitorClient
			.GetByNameAsync<DashboardGroup>(DashboardGroupName, default)
			.ConfigureAwait(false);

		dashboardGroup.Should().NotBeNull();

		dashboardGroup!
			.CustomProperties
			.Should()
			.Contain(x => x.Name == "TestToken" && x.Value == "TestValue");

		await LogicMonitorClient
			.DeleteAsync(dashboardGroup, default)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetDashboardGroupByFullPath()
	{
		var dashboardGroup = (await LogicMonitorClient
			.GetAllAsync<DashboardGroup>(default)
			.ConfigureAwait(false))[1];

		var dashboardGroupByPath = await LogicMonitorClient
			.GetDashboardGroupByFullPathAsync(dashboardGroup.FullPath, default)
			.ConfigureAwait(false);

		dashboardGroupByPath.Should().NotBeNull();
		dashboardGroupByPath.Id.Should().Be(dashboardGroup.Id);
	}

	[Fact]
	public async Task GetChildDashboardGroups()
	{
		var dashboardGroup = (await LogicMonitorClient
			.GetAllAsync<DashboardGroup>(default)
			.ConfigureAwait(false))[0];

		var dashboardChildren = await LogicMonitorClient
			.GetChildDashboardGroupsAsync(dashboardGroup.Id, new Filter<DashboardGroup>(), default)
			.ConfigureAwait(false);

		dashboardChildren.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetChildDashboards()
	{
		var dashboardGroup = (await LogicMonitorClient
			.GetAllAsync<DashboardGroup>(default)
			.ConfigureAwait(false))[0];

		var children = await LogicMonitorClient
			.GetChildDashboardsAsync(dashboardGroup.Id, new Filter<Dashboard>(), default)
			.ConfigureAwait(false);

		children.Items.Should().NotBeEmpty();
	}
}
