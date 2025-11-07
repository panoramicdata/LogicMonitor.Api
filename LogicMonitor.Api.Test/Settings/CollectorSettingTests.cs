namespace LogicMonitor.Api.Test.Settings;

public class CollectorSettingTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllCollectorGroupSettings()
	{
		var allCollectorGroupSettings = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(CancellationToken);
		allCollectorGroupSettings.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllCollectors()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken);
		collectors.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllCollectorsSettings()
	{
		var allCollectorSettings = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken);
		allCollectorSettings.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetCollectorGroupSettings()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(CancellationToken);
		var pulsantCollectorGroupSettings = await LogicMonitorClient
			.GetAsync<CollectorGroup>(collectorGroups[0].Id, CancellationToken);
		pulsantCollectorGroupSettings.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCollectorVersions_Unfiltered_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync(new(), CancellationToken);
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
				FilterItems =
				[
						new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true)
				]
			}, CancellationToken);
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
				FilterItems =
				[
					new Eq<CollectorVersion>(nameof(CollectorVersion.Mandatory), true)
				]
			}, CancellationToken);
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.IsStable.Should().BeTrue());
	}
}
