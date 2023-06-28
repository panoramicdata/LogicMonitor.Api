namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorGroupTests : TestWithOutput
{
	private const string TestName = "ApiTest";
	private const string TestDescription = "ApiTest Description";

	public CollectorGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>(default).ConfigureAwait(false);
		collectorGroups.Should().NotBeNull();
		collectorGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetPageAsync<CollectorGroup>(new Filter<CollectorGroup>(), $"setting/collector/groups", default).ConfigureAwait(false);
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
			FilterItems = new List<FilterItem<CollectorGroup>>
					{
						new Eq<CollectorGroup>(nameof(CollectorGroup.Name), TestName)
					}
		}, default).ConfigureAwait(false);

		foreach (var priorCollectorGroup in collectorGroups)
		{
			await LogicMonitorClient.DeleteAsync(priorCollectorGroup, cancellationToken: default).ConfigureAwait(false);
		}
		// There are now none with this name

		// Create one
		var newCollectorGroup = await LogicMonitorClient.CreateAsync(new CollectorGroupCreationDto
		{
			Name = TestName,
			Description = TestDescription,
			CustomProperties = new List<EntityProperty>
				{
					new EntityProperty { Name = "a", Value = "b" }
				}
		}, default).ConfigureAwait(false);
		newCollectorGroup.Should().NotBeNull();
		newCollectorGroup.Id.Should().NotBe(0);

		var newCollectorGroupRefetch = await LogicMonitorClient.GetAsync<CollectorGroup>(newCollectorGroup.Id, default).ConfigureAwait(false);
		newCollectorGroupRefetch.Should().NotBeNull();
		newCollectorGroupRefetch.Name.Should().NotBeNull();
		newCollectorGroupRefetch.Description.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNullOrEmpty();
		newCollectorGroupRefetch.CustomProperties.Should().HaveCount(1);
		newCollectorGroupRefetch.CustomProperties[0].Name.Should().Be("a");
		newCollectorGroupRefetch.CustomProperties[0].Value.Should().Be("b");

		// Put
		await LogicMonitorClient.PutAsync(newCollectorGroupRefetch, default).ConfigureAwait(false);

		// Delete
		await LogicMonitorClient.DeleteAsync(newCollectorGroupRefetch, cancellationToken: default).ConfigureAwait(false);
	}
}
