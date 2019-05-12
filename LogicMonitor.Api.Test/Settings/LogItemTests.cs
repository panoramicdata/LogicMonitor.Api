using LogicMonitor.Api.Logs;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class LogItemTests : TestWithOutput
	{
		public LogItemTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void Get()
		{
			var accessLogItems = await PortalClient.GetAllAsync<LogItem>().ConfigureAwait(false);

			// Make sure that some are returned
			Assert.True(accessLogItems.Count > 0);

			// TODO Make sure that all have Unique Ids
			//Assert.False(accessLogItems.Select(a => a.Id).HasDuplicates());

			Assert.True(accessLogItems.Count > 50);
		}

		[Fact]
		public async void GetPdl()
		{
			const int skip = 0;
			const int take = 1000;
			var accessLogItems = await PortalClient
				.GetLogItemsAsync(new LogFilter(
					skip,
					take,
					DateTime.UtcNow.AddDays(-2),
					DateTime.UtcNow,
					LogFilterSortOrder.HappenedOnAsc)).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.True(accessLogItems.Count > 0);

			// Make sure that all have Unique Ids
			Assert.False(accessLogItems.Select(a => a.Id).HasDuplicates());
		}
	}
}