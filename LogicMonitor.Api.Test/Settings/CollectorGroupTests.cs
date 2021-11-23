using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Filters;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings;

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
		Assert.NotNull(collectorGroups);
		Assert.NotEmpty(collectorGroups);
	}

	[Fact]
	public async void GetCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
		Assert.NotEmpty(collectorGroups);
		Assert.True(collectorGroups.All(cg => cg.Id != 0));
		Assert.True(collectorGroups.All(cg => cg.Name != null));
		// Bug in LogicMonitor's API
		// Assert.NotEqual(0, collectorGroups.TotalCount);
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
		Assert.NotNull(newCollectorGroup);
		Assert.NotEqual(0, newCollectorGroup.Id);

		var newCollectorGroupRefetch = await LogicMonitorClient.GetAsync<CollectorGroup>(newCollectorGroup.Id).ConfigureAwait(false);
		Assert.NotNull(newCollectorGroupRefetch);
		Assert.NotNull(newCollectorGroupRefetch.Name);
		Assert.NotNull(newCollectorGroupRefetch.Description);
		Assert.NotNull(newCollectorGroupRefetch.CustomProperties);
		Assert.NotEmpty(newCollectorGroupRefetch.CustomProperties);
		Assert.Single(newCollectorGroupRefetch.CustomProperties);
		Assert.Equal("a", newCollectorGroupRefetch.CustomProperties[0].Name);
		Assert.Equal("b", newCollectorGroupRefetch.CustomProperties[0].Value);

		// Put
		await LogicMonitorClient.PutAsync(newCollectorGroupRefetch).ConfigureAwait(false);

		// Delete
		await LogicMonitorClient.DeleteAsync(newCollectorGroupRefetch).ConfigureAwait(false);
	}
}
