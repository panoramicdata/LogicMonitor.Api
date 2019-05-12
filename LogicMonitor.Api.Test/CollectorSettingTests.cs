using LogicMonitor.Api.Collectors;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test
{
	public class CollectorSettingTests
		: TestWithOutput
	{
		public CollectorSettingTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllCollectorGroupSettings()
		{
			var allCollectorGroupSettings = await PortalClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
			Assert.NotNull(allCollectorGroupSettings);
			Assert.True(allCollectorGroupSettings.Count > 0);
		}

		[Fact]
		public async void GetAllCollectors()
		{
			var collectors = await PortalClient.GetAllAsync<Collector>().ConfigureAwait(false);
			Assert.NotNull(collectors);
		}

		[Fact]
		public async void GetAllCollectorsSettings()
		{
			var allCollectorSettings = await PortalClient.GetAllAsync<Collector>().ConfigureAwait(false);
			Assert.NotNull(allCollectorSettings);
			Assert.True(allCollectorSettings.Count > 0);
		}

		[Fact]
		public async void GetCollectorGroupSettings()
		{
			var collectorGroups = await PortalClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
			var pulsantCollectorGroupSettings = await PortalClient.GetAsync<CollectorGroup>(collectorGroups[0].Id).ConfigureAwait(false);
			Assert.NotNull(pulsantCollectorGroupSettings);
		}

		[Fact]
		public async void GetCollectorsSettings()
		{
			var collector = await PortalClient.GetAsync<Collector>(18).ConfigureAwait(false);
			Assert.NotNull(collector);
		}
	}
}