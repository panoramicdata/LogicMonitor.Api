namespace LogicMonitor.Api.Test.Settings;

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
		allCollectorGroupSettings.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void GetAllCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		collectors.Should().NotBeNull();
	}

	[Fact]
	public async void GetAllCollectorsSettings()
	{
		var allCollectorSettings = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		allCollectorSettings.Should().NotBeNullOrEmpty();
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
		pulsantCollectorGroupSettings.Should().NotBeNull();
	}

	[Fact]
	public async void GetCollectorVersions_Unfiltered_Succeeds()
	{
		var collectorVersions = await LogicMonitorClient
			.GetAllCollectorVersionsAsync()
			.ConfigureAwait(false);
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.MajorVersion.Should().NotBe(0));
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
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.IsStable.Should().BeTrue());
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
		collectorVersions.Should().NotBeNull();
		collectorVersions.Should().NotBeNullOrEmpty();
		collectorVersions.Should().AllSatisfy(collectorVersion => collectorVersion.IsStable.Should().BeTrue());
	}
}
