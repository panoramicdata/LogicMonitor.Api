using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test
{
	public class CollectorTests
		: TestWithOutput
	{
		public CollectorTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllCollectorGroups()
		{
			var collectorGroups = await PortalClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
			Assert.NotNull(collectorGroups);
			Assert.True(collectorGroups.Count > 0);

			// Re-fetch each
			foreach (var collectorGroup in collectorGroups)
			{
				var refetch = await PortalClient.GetAsync<CollectorGroup>(collectorGroup.Id).ConfigureAwait(false);
				Assert.NotNull(refetch);
			}
		}

		[Fact]
		public async void GetAllCollectors()
		{
			var collectors = await PortalClient.GetAllAsync<Collector>().ConfigureAwait(false);
			Assert.NotNull(collectors);
			Assert.True(collectors.Count > 0);

			// Re-fetch each
			foreach (var collector in collectors)
			{
				var refetch = await PortalClient.GetAsync<Collector>(collector.Id).ConfigureAwait(false);
				Assert.NotNull(refetch);
			}
		}

		[Fact]
		public async void RunDebugCommand()
		{
			var collectors = await PortalClient.GetAllAsync<Collector>().ConfigureAwait(false);
			var testCollector = collectors.Find(c => !c.IsDown);
			Assert.NotNull(testCollector);
			var response = await PortalClient.ExecuteDebugCommandAndWaitForResultAsync(testCollector.Id, "!ping 8.8.8.8").ConfigureAwait(false);
			Assert.NotNull(response);
			Logger.LogInformation(response.Output);
		}

		[Fact]
		public async void CreateCollectorDownloadAndDelete()
		{
			// Create the collector
			var collector = await PortalClient.CreateAsync(new CollectorCreationDto { Description = "UNIT TEST" }).ConfigureAwait(false);

			var tempFileInfo = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString());
			try
			{
				await PortalClient.DownloadCollector(
					collector.Id,
					tempFileInfo,
					CollectorPlatformAndArchitecture.Win64,
					CollectorDownloadType.Bootstrap,
					CollectorSize.Medium,
					28200).ConfigureAwait(false);
			}
			finally
			{
				// No need to do this as the collector has not been registered
				// await DefaultPortalClient.DeleteAsync(collector).ConfigureAwait(false);
				tempFileInfo.Delete();

				// Remove the collector from the API
				await PortalClient.DeleteAsync(collector).ConfigureAwait(false);
			}
		}
	}
}