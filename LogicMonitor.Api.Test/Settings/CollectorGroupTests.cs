namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private const string TestName = "ApiTest";
	private const string TestDescription = "ApiTest Description";

	[Fact]
	public async Task GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(true);
		collectorGroups.Should().NotBeNull();
		collectorGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient
			.GetPageAsync(new Filter<CollectorGroup>(), $"setting/collector/groups", default)
			.ConfigureAwait(true);
		collectorGroups.Items.Should().NotBeNullOrEmpty();
		collectorGroups.Items.Should().NotContain(cg => cg.Id == 0);
		collectorGroups.Items.Should().NotContain(cg => cg.Name == null);
		// Bug in LogicMonitor's API
		collectorGroups.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async Task GetAllCollectorGroupsWithSubUrl()
	{
		// Create a large number of CollectorGroups, all called "GetAllCollectorGroupsWithSubUrl_N" where N is a number
		var collectorGroupNames = Enumerable.Range(1, 100)
			.Select(i => $"GetAllCollectorGroupsWithSubUrl_{i}")
			.ToList();

		foreach (var collectorGroupName in collectorGroupNames)
		{
			await LogicMonitorClient.CreateAsync(new CollectorGroupCreationDto
			{
				Name = collectorGroupName,
				Description = "Description"
			}, default).ConfigureAwait(true);
		}

		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<JObject>($"setting/collector/groups", default);
		collectorGroups.Should().NotBeNullOrEmpty();

		// All should have the name property set
		foreach (var collectorGroup in collectorGroups)
		{
			collectorGroup["name"].Should().NotBeNull();
			collectorGroup["id"].Should().NotBeNull();
		}

		// Ensure that all the CollectorGroups we created are in the list
		var idsToDelete = new List<int>();
		foreach (var collectorGroupName in collectorGroupNames)
		{
			var collectorGroup = collectorGroups.FirstOrDefault(cg => cg["name"]!.Value<string>() == collectorGroupName);
			collectorGroup.Should().NotBeNull();
			idsToDelete.Add(collectorGroup["id"]!.Value<int>());
		}

		// Delete all the CollectorGroups we created
		foreach (var idToDelete in idsToDelete)
		{
			await LogicMonitorClient.DeleteAsync($"setting/collector/groups/{idToDelete}", default).ConfigureAwait(true);
		}
	}

	[Fact]
	public async Task CrudCollectorGroup()
	{
		// Try to get this item
		var collectorGroups = await LogicMonitorClient.GetAllAsync(new Filter<CollectorGroup>
		{
			FilterItems =
			[
				new Eq<CollectorGroup>(nameof(CollectorGroup.Name), TestName)
			]
		}, default).ConfigureAwait(true);

		foreach (var priorCollectorGroup in collectorGroups)
		{
			await LogicMonitorClient
				.DeleteAsync(priorCollectorGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}
		// There are now none with this name

		// Create one
		var newCollectorGroup = await LogicMonitorClient.CreateAsync(new CollectorGroupCreationDto
		{
			Name = TestName,
			Description = TestDescription,
			CustomProperties =
				[
					new EntityProperty { Name = "a", Value = "b" }
				]
		}, default).ConfigureAwait(true);
		newCollectorGroup.Should().NotBeNull();
		newCollectorGroup.Id.Should().NotBe(0);

		var newCollectorGroupRefetch = await LogicMonitorClient
			.GetAsync<CollectorGroup>(newCollectorGroup.Id, default)
			.ConfigureAwait(true);
		newCollectorGroupRefetch.Should().NotBeNull();
		newCollectorGroupRefetch.Name.Should().NotBeNull();
		newCollectorGroupRefetch.Description.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNullOrEmpty();
		newCollectorGroupRefetch.CustomProperties.Should().ContainSingle();
		newCollectorGroupRefetch.CustomProperties[0].Name.Should().Be("a");
		newCollectorGroupRefetch.CustomProperties[0].Value.Should().Be("b");

		// Put
		await LogicMonitorClient
			.PutAsync(newCollectorGroupRefetch, default)
			.ConfigureAwait(true);

		// Delete
		await LogicMonitorClient
			.DeleteAsync(newCollectorGroupRefetch, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
