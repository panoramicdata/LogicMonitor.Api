namespace LogicMonitor.Api.Test.EventLogs;
public class GetFilteredAuditEventTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private readonly DateTime _endDateTimeUtc = DateTime.UtcNow;
	private readonly DateTime _startDateTimeUtc = DateTime.UtcNow.AddHours(-1);

	[Fact]
	public async Task GetUsernameFilteredEvents()
	{
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, _startDateTimeUtc, _endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc), default)
			.ConfigureAwait(true);

		unfilteredLogItems.Count.Should().BePositive();

		var filteredLogItemsSystemActiveDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				_startDateTimeUtc,
				_endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AActiveDiscovery\"" },
			default
			)
			.ConfigureAwait(true);

		var filteredLogItemsSystemActiveDiscoveryCount = filteredLogItemsSystemActiveDiscovery.Count;

		var filteredLogItemsSystemAppliesTo = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				_startDateTimeUtc,
				_endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"" },
			default
			)
			.ConfigureAwait(true);

		var filteredLogItemsSystemAppliesToCount = filteredLogItemsSystemAppliesTo.Count;

		var filteredLogItemsSystemAppliesToAndSystemDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				filteredLogItemsSystemAppliesToCount + filteredLogItemsSystemActiveDiscoveryCount,
				_startDateTimeUtc,
				_endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"|\"System%3AActiveDiscovery\"" },
			default
			)
			.ConfigureAwait(true);

		var filteredLogItemsSystemAppliesToAndSystemDiscoveryCount = filteredLogItemsSystemAppliesToAndSystemDiscovery.Count;

		filteredLogItemsSystemAppliesToAndSystemDiscoveryCount.Should().Be(
			filteredLogItemsSystemActiveDiscoveryCount + filteredLogItemsSystemAppliesToCount
		);

	}

	[Fact]
	public async Task GetTextFilteredEvents()
	{
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, _startDateTimeUtc, _endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc),
			default
			)
			.ConfigureAwait(true);

		var unfilteredLogItemsCount = unfilteredLogItems.Count;

		unfilteredLogItemsCount.Should().NotBe(0);

		var filteredLogItemsHealthTextExcluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				_startDateTimeUtc,
				_endDateTimeUtc,
				LogFilterSortOrder.HappenedOnDesc)
			{ TextFilter = "\"* AND NOT *health*\"" },
			default
			)
			.ConfigureAwait(true);

		var filteredLogItemsHealthTextExcludedCount = filteredLogItemsHealthTextExcluded.Count;

		var filteredLogItemsHealthTextIncluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				_startDateTimeUtc,
				_endDateTimeUtc,
				LogFilterSortOrder.HappenedOnDesc)
			{ TextFilter = "\"*health*\"" },
			default
			)
			.ConfigureAwait(true);

		var filteredLogItemsHealthTextIncludedCount = filteredLogItemsHealthTextIncluded.Count;

		(unfilteredLogItemsCount - filteredLogItemsHealthTextExcludedCount).Should().Be(filteredLogItemsHealthTextIncludedCount);
	}
}
