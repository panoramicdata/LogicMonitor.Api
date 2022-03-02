using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Settings;

public class LogItemTests : TestWithOutput
{
	public LogItemTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void Get()
	{
		var accessLogItems = await LogicMonitorClient.GetAllAsync(new Filter<LogItem>
		{
			FilterItems = new List<FilterItem<LogItem>>
				{
					new Gt<LogItem>(nameof(LogItem.HappenedOnTimeStampUtc), DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds())
				},
			Order = new Order<LogItem>
			{
				Property = nameof(LogItem.HappenedOnTimeStampUtc),
				Direction = OrderDirection.Asc
			}
		}).ConfigureAwait(false);

		// Make sure that some are returned
		(accessLogItems.Count > 0).Should().BeTrue();

		// Make sure that all have Unique Ids
		accessLogItems.Select(a => a.Id)
			.HasDuplicates()
			.Should()
			.BeFalse();
	}

	[Fact]
	public async void GetPdl()
	{
		const int skip = 0;
		const int take = 1000;
		var accessLogItems = await LogicMonitorClient
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
