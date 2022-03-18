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
	public async void GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
		collectorGroups.Should().NotBeNull();
		collectorGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void GetCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
		collectorGroups.Should().NotBeNullOrEmpty();
		Assert.True(collectorGroups.All(cg => cg.Id != 0));
		Assert.True(collectorGroups.All(cg => cg.Name != null));
		// Bug in LogicMonitor's API
		// collectorGroups.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async void CrudCollectorGroup()
	{
		// Try to get this item
		var collectorGroups = await LogicMonitorClient.GetAllAsync(new Filter<CollectorGroup>
		{
			FilterItems = new List<FilterItem<CollectorGroup>>
					{
						new Eq<CollectorGroup>(nameof(CollectorGroup.Name), TestName)
					}
		}).ConfigureAwait(false);

		foreach (var priorCollectorGroup in collectorGroups)
		{
			await LogicMonitorClient.DeleteAsync(priorCollectorGroup).ConfigureAwait(false);
		}
		// There are now none with this name

		// Create one
		var newCollectorGroup = await LogicMonitorClient.CreateAsync(new CollectorGroupCreationDto
		{
			Name = TestName,
			Description = TestDescription,
			CustomProperties = new List<Property>
				{
					new Property { Name = "a", Value = "b" }
				}
		}).ConfigureAwait(false);
		newCollectorGroup.Should().NotBeNull();
		newCollectorGroup.Id.Should().NotBe(0);

		var newCollectorGroupRefetch = await LogicMonitorClient.GetAsync<CollectorGroup>(newCollectorGroup.Id).ConfigureAwait(false);
		newCollectorGroupRefetch.Should().NotBeNull();
		newCollectorGroupRefetch.Name.Should().NotBeNull();
		newCollectorGroupRefetch.Description.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNull();
		newCollectorGroupRefetch.CustomProperties.Should().NotBeNullOrEmpty();
		Assert.Single(newCollectorGroupRefetch.CustomProperties);
		newCollectorGroupRefetch.CustomProperties[0].Name.Should().Be("a");
		newCollectorGroupRefetch.CustomProperties[0].Value.Should().Be("b");

		// Put
		await LogicMonitorClient.PutAsync(newCollectorGroupRefetch).ConfigureAwait(false);

		// Delete
		await LogicMonitorClient.DeleteAsync(newCollectorGroupRefetch).ConfigureAwait(false);
	}
}
