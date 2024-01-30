namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorGroupTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
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
			.GetPageAsync<CollectorGroup>(new Filter<CollectorGroup>(), $"setting/collector/groups", default)
			.ConfigureAwait(true);
		collectorGroups.Items.Should().NotBeNullOrEmpty();
		collectorGroups.Items.Should().NotContain(cg => cg.Id == 0);
		collectorGroups.Items.Should().NotContain(cg => cg.Name == null);
		// Bug in LogicMonitor's API
		collectorGroups.TotalCount.Should().NotBe(0);
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
