using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Settings;

public class LogItemTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task Get()
	{
		var accessLogItems = await LogicMonitorClient.GetAllAsync(new Filter<LogItem>
		{
			Take = 100,
			FilterItems =
			[
				new Gt<LogItem>(nameof(LogItem.HappenedOnTimeStampUtc), DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds())
			],
			Order = new Order<LogItem>
			{
				Property = nameof(LogItem.HappenedOnTimeStampUtc),
				Direction = OrderDirection.Asc
			}
		}, CancellationToken);

		// Make sure that some are returned
		(accessLogItems.Count > 0).Should().BeTrue();

		// Make sure that all have Unique Ids
		accessLogItems.Select(a => a.Id)
			.HasDuplicates()
			.Should()
			.BeFalse();
	}

	[Fact]
	public async Task GetPdl()
	{
		const int skip = 0;
		const int take = 1000;
		var accessLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				skip,
				take,
				DateTime.UtcNow.AddDays(-2),
				DateTime.UtcNow,
				LogFilterSortOrder.HappenedOnAsc), CancellationToken);

		// Make sure that some are returned
		accessLogItems.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		accessLogItems.Select(a => a.Id).HasDuplicates().Should().BeFalse();
	}
}
