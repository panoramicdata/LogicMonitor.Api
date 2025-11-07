namespace LogicMonitor.Api.Test.EventLogs;
public class GetFilteredAuditEventTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private readonly DateTime _endDateTimeUtc = DateTime.UtcNow;
	private readonly DateTime _startDateTimeUtc = DateTime.UtcNow.AddHours(-1);

	[Fact]
	public async Task GetUsernameFilteredEvents()
	{
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, _startDateTimeUtc, _endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc), CancellationToken);

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
			;

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
			;

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
			;

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
			;

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
			;

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
			;

		var filteredLogItemsHealthTextIncludedCount = filteredLogItemsHealthTextIncluded.Count;

		(unfilteredLogItemsCount - filteredLogItemsHealthTextExcludedCount).Should().Be(filteredLogItemsHealthTextIncludedCount);
	}
}
