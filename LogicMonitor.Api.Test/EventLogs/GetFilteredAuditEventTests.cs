﻿using System.Globalization;

namespace LogicMonitor.Api.Test.EventLogs;
public class GetFilteredAuditEventTests : TestWithOutput
{
	public GetFilteredAuditEventTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetUsernameFilteredEvents()
	{
		var startDateTimeUtc = DateTime.Parse("2022-06-17 00:00:00 +01:00", CultureInfo.InvariantCulture);
		var endDateTimeUtc = DateTime.Parse("2022-06-20 09:00:00 +01:00", CultureInfo.InvariantCulture);
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc), CancellationToken.None)
			.ConfigureAwait(false);

		unfilteredLogItems.Should().HaveCount(75);

		var filteredLogItemsSystemActiveDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AActiveDiscovery\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsSystemActiveDiscovery.Should().HaveCount(13);

		var filteredLogItemsSystemAppliesTo = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsSystemAppliesTo.Should().HaveCount(40);

		var filteredLogItemsSystemAppliesToAndSystemDiscovery = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ UsernameFilter = "\"System%3AAppliesTo\"|\"System%3AActiveDiscovery\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsSystemAppliesToAndSystemDiscovery.Should().HaveCount(53);

	}

	[Fact]
	public async Task GetTextFilteredEvents()
	{
		var startDateTimeUtc = DateTime.Parse("2022-06-17 00:00:00 +01:00", CultureInfo.InvariantCulture);
		var endDateTimeUtc = DateTime.Parse("2022-06-20 09:00:00 +01:00", CultureInfo.InvariantCulture);
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(0, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc),
			CancellationToken.None
			)
			.ConfigureAwait(false);

		unfilteredLogItems.Should().HaveCount(75);

		var filteredLogItemsHealthTextExcluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ TextFilter = "\"* AND NOT *health*\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsHealthTextExcluded.Should().HaveCount(61);

		var filteredLogItemsHealthTextIncluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ TextFilter = "\"*health*\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsHealthTextIncluded.Should().HaveCount(14);
	}

	[Fact]
	public async Task GetUsernameAndTextFilteredEvents()
	{
		var startDateTimeUtc = DateTime.Parse("2022-06-17 00:00:00 +01:00", CultureInfo.InvariantCulture);
		var endDateTimeUtc = DateTime.Parse("2022-06-20 09:00:00 +01:00", CultureInfo.InvariantCulture);
		var unfilteredLogItems = await LogicMonitorClient
			.GetLogItemsAsync(
				new LogFilter(0, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc),
				CancellationToken.None)
			.ConfigureAwait(false);

		unfilteredLogItems.Should().HaveCount(75);

		var filteredLogItemsHealthTextIncluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{ TextFilter = "\"*health*\"" },
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsHealthTextIncluded.Should().HaveCount(14);

		var filteredLogItemsHealthTextFilteredUsernameIncluded = await LogicMonitorClient
			.GetLogItemsAsync(new LogFilter(
				0,
				300,
				startDateTimeUtc,
				endDateTimeUtc,
				LogFilterSortOrder.HappenedOnAsc)
			{
				UsernameFilter = "\"System%3AAppliesTo\"|\"System%3AActiveDiscovery\"",
				TextFilter = "\"*health*\""
			},
			CancellationToken.None
			)
			.ConfigureAwait(false);

		filteredLogItemsHealthTextFilteredUsernameIncluded.Should().HaveCount(2);
	}
}