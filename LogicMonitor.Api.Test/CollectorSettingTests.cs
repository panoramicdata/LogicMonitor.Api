using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Filters;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test;

public class CollectorSettingTests
	: TestWithOutput
{
	public CollectorSettingTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllCollectorGroupSettings()
	{
		var allCollectorGroupSettings = await LogicMonitorClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
		Assert.NotNull(allCollectorGroupSettings);
		Assert.True(allCollectorGroupSettings.Count > 0);
	}

	[Fact]
	public async void GetAllCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		Assert.NotNull(collectors);
	}

	[Fact]
	public async void GetAllCollectorsSettings()
	{
		var allCollectorSettings = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		Assert.NotNull(allCollectorSettings);
		Assert.True(allCollectorSettings.Count > 0);
	}

	[Fact]
	public async void GetCollectorGroupSettings()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>()
			.ConfigureAwait(false);
		var pulsantCollectorGroupSettings = await LogicMonitorClient
			.GetAsync<CollectorGroup>(collectorGroups[0].Id)
			.ConfigureAwait(false);
		Assert.NotNull(pulsantCollectorGroupSettings);
	}

	[Fact]
	public async void GetCollectorVersions_Unfiltered_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync()
			.ConfigureAwait(false);
		Assert.NotNull(collectorVersions);
		Assert.NotEmpty(collectorVersions);
		Assert.All(collectorVersions, collectorVersion => Assert.NotEqual(0, collectorVersion.MajorVersion));
	}

	[Fact]
	public async void GetCollectorVersions_FilteredStable_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true)
				}
			})
			.ConfigureAwait(false);
		Assert.NotNull(collectorVersions);
		Assert.NotEmpty(collectorVersions);
		Assert.All(collectorVersions, collectorVersion => Assert.True(collectorVersion.IsStable));
	}

	[Fact]
	public async void GetCollectorVersions_FilteredMandatory_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.Mandatory), true)
				}
			})
			.ConfigureAwait(false);
		Assert.NotNull(collectorVersions);
		Assert.NotEmpty(collectorVersions);
		Assert.All(collectorVersions, collectorVersion => Assert.True(collectorVersion.IsStable));
	}
}
