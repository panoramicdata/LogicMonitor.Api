namespace LogicMonitor.Api.Test.Resources;

public class ResourceGroupClusterAlertConfigTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TestGroupFullPath = "NUG - LogicMonitor.Api Nuget/Cluster Alert Tests";

	private async Task<ResourceGroup> EnsureTestGroupExistsAsync()
	{
		var group = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(TestGroupFullPath, CancellationToken);

		if (group is not null)
		{
			return group;
		}

		var parent = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("NUG - LogicMonitor.Api Nuget", CancellationToken)
			?? throw new InvalidOperationException("Parent group 'NUG - LogicMonitor.Api Nuget' not found.");

		return await LogicMonitorClient.CreateAsync(
			new ResourceGroupCreationDto
			{
				ParentId = $"{parent.Id}",
				Name = "Cluster Alert Tests"
			},
			CancellationToken);
	}

	[Fact]
	public async Task CreateGetUpdatePatchDelete_ClusterAlertConfig_Succeeds()
	{
		var group = await EnsureTestGroupExistsAsync();

		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", CancellationToken)
			?? throw new InvalidOperationException("DataSource 'Ping' not found.");

		// --- CREATE ---
		var config = new ResourceGroupClusterAlertConfig
		{
			DataSourceId = dataSource.Id,
			CountBy = "host",
			MinAlertLevel = 2,
			AlertExpression = ">= 1",
			ThresholdType = "absolute",
			SuppressIndividualAlerts = false,
			DisableAlerting = false
		};

		var created = await LogicMonitorClient
			.CreateResourceGroupClusterAlertConfigAsync(group.Id, config, CancellationToken);

		created.Should().NotBeNull();
		created.Id.Should().BePositive();
		created.ResourceGroupId.Should().Be(group.Id);
		created.DataSourceId.Should().Be(dataSource.Id);
		created.CountBy.Should().Be("host");

		try
		{
			// --- GET ALL ---
			var all = await LogicMonitorClient
				.GetAllResourceGroupClusterAlertConfigsAsync(group.Id, CancellationToken);

			all.Should().NotBeEmpty();
			all.Should().AllSatisfy(c => c.ResourceGroupId.Should().Be(group.Id));
			all.Should().Contain(c => c.Id == created.Id);

			// --- GET PAGE ---
			var page = await LogicMonitorClient
				.GetResourceGroupClusterAlertConfigsPageAsync(group.Id, null, CancellationToken);

			page.Should().NotBeNull();
			page.Items.Should().NotBeEmpty();
			page.Items.Should().AllSatisfy(c => c.ResourceGroupId.Should().Be(group.Id));

			// --- GET BY ID ---
			var fetched = await LogicMonitorClient
				.GetResourceGroupClusterAlertConfigAsync(group.Id, created.Id, CancellationToken);

			fetched.Should().NotBeNull();
			fetched.Id.Should().Be(created.Id);
			fetched.ResourceGroupId.Should().Be(group.Id);
			fetched.DataSourceId.Should().Be(dataSource.Id);

			// --- UPDATE (PUT) ---
			fetched.MinAlertLevel = 3;
			fetched.AlertExpression = ">= 2";
			await LogicMonitorClient
				.UpdateResourceGroupClusterAlertConfigAsync(group.Id, fetched, CancellationToken);

			var afterPut = await LogicMonitorClient
				.GetResourceGroupClusterAlertConfigAsync(group.Id, created.Id, CancellationToken);
			afterPut.MinAlertLevel.Should().Be(3);
			afterPut.AlertExpression.Should().Be(">= 2");

			// --- PATCH ---
			await LogicMonitorClient
				.PatchResourceGroupClusterAlertConfigAsync(
					group.Id,
					created.Id,
					new Dictionary<string, object> { ["disableAlerting"] = true },
					CancellationToken);

			var afterPatch = await LogicMonitorClient
				.GetResourceGroupClusterAlertConfigAsync(group.Id, created.Id, CancellationToken);
			afterPatch.DisableAlerting.Should().BeTrue();
		}
		finally
		{
			// --- DELETE ---
			await LogicMonitorClient
				.DeleteResourceGroupClusterAlertConfigAsync(group.Id, created.Id, CancellationToken);

			var remaining = await LogicMonitorClient
				.GetAllResourceGroupClusterAlertConfigsAsync(group.Id, CancellationToken);
			remaining.Should().NotContain(c => c.Id == created.Id);
		}
	}
}
