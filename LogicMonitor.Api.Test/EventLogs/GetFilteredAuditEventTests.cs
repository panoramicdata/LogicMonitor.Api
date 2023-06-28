namespace LogicMonitor.Api.Test.EventLogs;
public class GetFilteredAuditEventTests : TestWithOutput
{
	private readonly DateTime endDateTimeUtc = DateTime.UtcNow;
	private readonly DateTime startDateTimeUtc = DateTime.UtcNow.AddHours(-1);

	public GetFilteredAuditEventTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetUsernameFilteredEvents()
	{
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc), default)
			.ConfigureAwait(false);

		unfilteredLogItems.Count.Should().BePositive();

		var filteredLogItemsSystemActiveDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AActiveDiscovery\"" },
			default
			)
			.ConfigureAwait(false);

		var filteredLogItemsSystemActiveDiscoveryCount = filteredLogItemsSystemActiveDiscovery.Count;

		var filteredLogItemsSystemAppliesTo = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"" },
			default
			)
			.ConfigureAwait(false);

		var filteredLogItemsSystemAppliesToCount = filteredLogItemsSystemAppliesTo.Count;

		var filteredLogItemsSystemAppliesToAndSystemDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				filteredLogItemsSystemAppliesToCount + filteredLogItemsSystemActiveDiscoveryCount,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"|\"System%3AActiveDiscovery\"" },
			default
			)
			.ConfigureAwait(false);

		var filteredLogItemsSystemAppliesToAndSystemDiscoveryCount = filteredLogItemsSystemAppliesToAndSystemDiscovery.Count;

		filteredLogItemsSystemAppliesToAndSystemDiscoveryCount.Should().Be(
			filteredLogItemsSystemActiveDiscoveryCount + filteredLogItemsSystemAppliesToCount
		);

	}

	[Fact]
	public async Task GetTextFilteredEvents()
	{
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc),
			default
			)
			.ConfigureAwait(false);

		var unfilteredLogItemsCount = unfilteredLogItems.Count;

		unfilteredLogItemsCount.Should().NotBe(0);

		var filteredLogItemsHealthTextExcluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnDesc)
			{ TextFilter = "\"* AND NOT *health*\"" },
			default
			)
			.ConfigureAwait(false);

		var filteredLogItemsHealthTextExcludedCount = filteredLogItemsHealthTextExcluded.Count;

		var filteredLogItemsHealthTextIncluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnDesc)
			{ TextFilter = "\"*health*\"" },
			default
			)
			.ConfigureAwait(false);

		var filteredLogItemsHealthTextIncludedCount = filteredLogItemsHealthTextIncluded.Count;

		(unfilteredLogItemsCount - filteredLogItemsHealthTextExcludedCount).Should().Be(filteredLogItemsHealthTextIncludedCount);
	}
}
