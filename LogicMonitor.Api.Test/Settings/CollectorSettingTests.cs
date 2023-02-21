namespace LogicMonitor.Api.Test.Settings;

public class CollectorSettingTests
	: TestWithOutput
{
	public CollectorSettingTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllCollectorGroupSettings()
	{
		var allCollectorGroupSettings = await LogicMonitorClient.GetAllAsync<CollectorGroup>(default).ConfigureAwait(false);
		allCollectorGroupSettings.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(default).ConfigureAwait(false);
		collectors.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllCollectorsSettings()
	{
		var allCollectorSettings = await LogicMonitorClient.GetAllAsync<Collector>(default).ConfigureAwait(false);
		allCollectorSettings.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetCollectorGroupSettings()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(false);
		var pulsantCollectorGroupSettings = await LogicMonitorClient
			.GetAsync<CollectorGroup>(collectorGroups[0].Id, default)
			.ConfigureAwait(false);
		pulsantCollectorGroupSettings.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCollectorVersions_Unfiltered_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(cancellationToken: default)
			.ConfigureAwait(false);
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.MajorVersion.Should().NotBe(0));
	}

	[Fact]
	public async Task GetCollectorVersions_FilteredStable_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true)
				}
			}, default)
			.ConfigureAwait(false);
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.IsStable.Should().BeTrue());
	}

	[Fact]
	public async Task GetCollectorVersions_FilteredMandatory_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.Mandatory), true)
				}
			}, default)
			.ConfigureAwait(false);
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.IsStable.Should().BeTrue());
	}
}
