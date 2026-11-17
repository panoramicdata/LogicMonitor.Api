using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Alerts;

public partial class AlertTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TestNoteComment = "Test Note";
	private static readonly DateTime _endDateTime = DateTime.UtcNow.Date;
	private static readonly int _startDateTimeSeconds = _endDateTime.AddDays(-7).SecondsSinceTheEpoch();
	private static readonly int _endDateTimeSeconds = _endDateTime.SecondsSinceTheEpoch();

	private static void CheckAlertsAreValid(List<Alert> alerts)
	{
		// Make sure that all have Unique Ids
		foreach (var alertType in Enum.GetValues<AlertType>())
		{
			alerts
				.Where(a => a.AlertType == alertType)
				.Select(a => a.Id)
				.HasDuplicates()
				.Should()
				.BeFalse();
		}
	}

	[Fact]
	public async Task ClosedAlerts()
	{
		var closedItemsFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Eq<Alert>(nameof(Alert.IsCleared), "*"),
				new Gt<Alert>(nameof(Alert.EndOnSeconds), DaysAgoAsUnixSeconds(10)),
			],
			Order = new Order<Alert>
			{
				Property = nameof(Alert.EndOnSeconds),
				Direction = OrderDirection.Desc
			},
			Take = 300
		};
		var alerts = await LogicMonitorClient
			.GetAllAsync(closedItemsFilter, CancellationToken);
		alerts.Should().NotBeNull();
		alerts.Should().NotBeEmpty();
		alerts.Should().NotContain(a => a.EndOnSeconds == 0);
	}

	[Fact]
	public async Task AddNoteToAlert()
	{
		var noNoteItemsFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Eq<Alert>(nameof(Alert.AckComment), string.Empty),
			],
			Properties =
			[
				nameof(Alert.Id),
				nameof(Alert.InternalId),
			],
			Take = 1
		};
		var alerts = await LogicMonitorClient
			.GetAllAsync(noNoteItemsFilter, CancellationToken);
		alerts.Should().NotBeNull();
		alerts.Should().ContainSingle();

		var alert = alerts[0];
		alert.AckComment.Should().Be("");

		// Add a note
		await LogicMonitorClient
			.SetAlertNoteAsync([alert.Id], TestNoteComment, CancellationToken);

		var refetchedAlert = await LogicMonitorClient
			.GetAlertAsync(alert.Id, CancellationToken);

		refetchedAlert.Should().NotBeNull();
		refetchedAlert.AckComment.Should().Be(TestNoteComment);
	}

	[Fact]
	public async Task AlertLevelPropertyIsPopulated()
	{
		var closedItemsFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Eq<Alert>(nameof(Alert.IsCleared), "*"),
				new Gt<Alert>(nameof(Alert.EndOnSeconds), DaysAgoAsUnixSeconds(10)),
			],
			Properties =
			[
				nameof(Alert.EndOnSeconds),
				nameof(Alert.Severity)
			],
			Order = new Order<Alert>
			{
				Property = nameof(Alert.EndOnSeconds),
				Direction = OrderDirection.Desc
			},
			Take = 300
		};
		var alerts = await LogicMonitorClient
			.GetAllAsync(closedItemsFilter, CancellationToken);
		alerts.Should().NotBeNull();
		alerts.Should().NotBeEmpty();
		alerts.Should().NotContain(a => a.EndOnSeconds == 0);
		alerts.Should().NotContain(a => a.AlertLevel == AlertLevel.Unknown);
	}

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetAlerts_SdtsMatchRequest()
	{
		// This unit test currently fails horribly due to some inability of the LogicMonitor API to sort the Alerts by Id descending

		// Arrange
		var allFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.All,
			StartEpochIsAfter = _startDateTimeSeconds,
			StartEpochIsBefore = _endDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};
		var sdtFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.Sdt,
			StartEpochIsAfter = _startDateTimeSeconds,
			StartEpochIsBefore = _endDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};
		var nonSdtFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.NonSdt,
			StartEpochIsAfter = _startDateTimeSeconds,
			StartEpochIsBefore = _endDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};

		// Act
		var allAlerts = await LogicMonitorClient
			.GetAlertsAsync(allFilter, CancellationToken);
		var sdtAlerts = await LogicMonitorClient
			.GetAlertsAsync(sdtFilter, CancellationToken);
		var nonSdtAlerts = await LogicMonitorClient
			.GetAlertsAsync(nonSdtFilter, CancellationToken);

		// Assert

		// All the ids should be unique
		allAlerts.Should().OnlyHaveUniqueItems(a => a.Id);
		sdtAlerts.Should().OnlyHaveUniqueItems(a => a.Id);
		nonSdtAlerts.Should().OnlyHaveUniqueItems(a => a.Id);

		// Alert counts should add up
		(sdtAlerts.Count + nonSdtAlerts.Count).Should().Be(allAlerts.Count);

		// Alerts have the expected SDT status
		sdtAlerts.Should().AllSatisfy(a => a.InScheduledDownTime.Should().BeTrue());
		nonSdtAlerts.Should().AllSatisfy(a => a.InScheduledDownTime.Should().BeFalse());
	}

	[Fact]
	public async Task GetAlertsAndCheckUnique()
	{
		var startEpoch = DateTime.UtcNow.AddDays(-1).SecondsSinceTheEpoch();
		var alertList = await LogicMonitorClient
			.GetRestAlertsWithV84BugAsync(new AlertFilter { StartEpochIsAfter = startEpoch }, TimeSpan.FromHours(8));

		var unique = true;
		foreach (var alert in alertList)
		{
			if (alertList.Count(a => a.Id == alert.Id) > 1)
			{
				unique = false;
			}
		}

		unique.Should().BeTrue();
	}

	[Fact]
	public async Task GetAlertsFilteredByResource()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var startUtcIsBefore = new DateTime(2018, 1, 1);
		foreach (var alertType in new List<AlertType> { AlertType.DataSource, AlertType.EventSource })
		{
			var alertTypes = new List<AlertType> { alertType };
			var alertFilter = new AlertFilter
			{
				AlertTypes = alertTypes,
				MonitorObjectId = device.Id,
				StartUtcIsBefore = startUtcIsBefore
			};
			var alerts = await LogicMonitorClient
				.GetAlertsAsync(alertFilter, CancellationToken);
			CheckAlertsAreValid(alerts);

			// Make sure there are no alerts for hosts not mentioned by the hostFilter
			alerts.Should().AllSatisfy(alert =>
			{
				device.DisplayName.Should().Be(alert.MonitorObjectName);
				alertTypes.Should().Contain(alert.AlertType);
				(alert.StartOnUtc < startUtcIsBefore).Should().BeTrue();
			});
		}
	}

	[Fact]
	public async Task GetAlertsFilteredByResourceRest()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var resourceGroupFullPathFilter = new List<string> { "Collectors*" };
		const string dataSourceNameFilter = "Volume Usage-";
		const string dataSourceInstanceNameFilter = @"WinVolumeUsage-C:\\";
		const string dataPointNameFilter = "Capacity";
		const AckFilter ackFilter = AckFilter.Acked;
		const SdtFilter sdtFilter = SdtFilter.Sdt;
		var levelsFilter = new List<AlertLevel> { AlertLevel.Error, AlertLevel.Critical };
		bool? includeClearedFilter = true;
		var utcNow = DateTime.UtcNow;

		var alertFilter = new AlertFilter
		{
			MonitorObjectGroupFullPaths = resourceGroupFullPathFilter,
			MonitorObjectName = device.DisplayName,
			ResourceTemplateName = dataSourceNameFilter,
			DataPointName = dataPointNameFilter,
			InstanceName = dataSourceInstanceNameFilter,
			AckFilter = ackFilter,
			SdtFilter = sdtFilter,
			Levels = levelsFilter,
			IncludeCleared = includeClearedFilter,
			StartEpochIsAfter = utcNow.AddDays(-1).SecondsSinceTheEpoch(),
			StartEpochIsBefore = utcNow.SecondsSinceTheEpoch()
		};
		var alerts = await LogicMonitorClient
			.GetAlertsAsync(alertFilter, CancellationToken);
		CheckAlertsAreValid(alerts);

		// Make sure there are no alerts for hosts not mentioned by the hostFilter
		alerts.Should().AllSatisfy(alert => device.DisplayName.Should().Be(alert.MonitorObjectName));
		alerts.Should().OnlyContain(alert => alert.InstanceName.StartsWith(dataSourceNameFilter, StringComparison.Ordinal));
		alerts.Should().OnlyContain(alert => alert.DataPointName.StartsWith(dataPointNameFilter, StringComparison.Ordinal));
	}

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetAlertsIncludeInactiveShouldWork()
	{
		// IncludeCleared set false should bring back only active alerts
		var alertFilterNotIncludingCleared = new AlertFilter
		{
			IncludeCleared = false,
			AckFilter = AckFilter.All,
			AlertRuleName = "",
			StartEpochIsBefore = DateTime.UtcNow.AddMinutes(-1).SecondsSinceTheEpoch(),
			SdtFilter = SdtFilter.All,
			StartEpochIsAfter = DateTime.UtcNow.AddDays(-7).SecondsSinceTheEpoch(),
			OrderByProperty = nameof(Alert.StartOnSeconds),
			OrderDirection = OrderDirection.Desc
		};
		var alertsNotIncludingCleared = await LogicMonitorClient
			.GetAlertsAsync(alertFilterNotIncludingCleared, CancellationToken);
		alertsNotIncludingCleared.Should().HaveCountGreaterThan(0);
		alertsNotIncludingCleared.Should().OnlyContain(a => a.IsActive);

		// IncludeCleared set true should bring back active AND non-active alerts
		var alertFilterIncludingCleared = new AlertFilter
		{
			IncludeCleared = true,
			AckFilter = AckFilter.All,
			AlertRuleName = "",
			StartEpochIsBefore = DateTime.UtcNow.AddMinutes(-1).SecondsSinceTheEpoch(),
			SdtFilter = SdtFilter.All,
			StartEpochIsAfter = DateTime.UtcNow.AddDays(-7).SecondsSinceTheEpoch(),
			OrderByProperty = nameof(Alert.StartOnSeconds),
			OrderDirection = OrderDirection.Desc
		};
		var alertsIncludingCleared = await LogicMonitorClient
			.GetAlertsAsync(alertFilterIncludingCleared, CancellationToken);
		alertsIncludingCleared.Count.Should().BePositive();
		alertsIncludingCleared.Should().Contain(a => !a.IsActive);
		alertsIncludingCleared.Should().Contain(a => a.IsActive);
	}

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetFilteredAlerts_LevelsMatch()
	{
		var unfilteredAlerts =
			await LogicMonitorClient
				.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = _startDateTimeSeconds, StartEpochIsBefore = _endDateTimeSeconds }, CancellationToken);
		var criticalAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical]
			}, CancellationToken);
		var errorAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical, AlertLevel.Error]
			}, CancellationToken);
		var warningAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning]
			}, CancellationToken);

		// Ensure that all alerts are at the appropriate level
		unfilteredAlerts.Should().AllSatisfy(a => (a.AlertLevel >= AlertLevel.Error).Should().BeTrue());
		criticalAlerts.Should().AllSatisfy(a => (a.AlertLevel >= AlertLevel.Critical).Should().BeTrue());
		errorAlerts.Should().AllSatisfy(a => (a.AlertLevel >= AlertLevel.Error).Should().BeTrue());
		warningAlerts.Should().AllSatisfy(a => (a.AlertLevel >= AlertLevel.Warning).Should().BeTrue());

		// Ensure that the counts make sense
		unfilteredAlerts.Should().HaveCount(errorAlerts.Count);
		criticalAlerts.Count.Should().BeLessThanOrEqualTo(errorAlerts.Count);
		errorAlerts.Count.Should().BeLessThanOrEqualTo(warningAlerts.Count);
	}

	[Fact]
	public async Task GetFilteredAlertsBroken_Rm1959()
	{
		var alerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			AckFilter = AckFilter.All,
			StartEpochIsBefore = _endDateTimeSeconds,
			IncludeCleared = false,
			Levels = [AlertLevel.Critical, AlertLevel.Error],
			NeedMessage = true,
			OrderDirection = OrderDirection.Desc,
			SdtFilter = SdtFilter.NonSdt,
			StartEpochIsAfter = _startDateTimeSeconds,
			Take = 1
		}, CancellationToken);
		CheckAlertsAreValid(alerts);
	}

	// Time needs to be changed
	[Fact]
	public async Task GetFilteredAlertsDefaultPeriod()
	{
		var alerts =
			await LogicMonitorClient
				.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = _startDateTimeSeconds, StartEpochIsBefore = _endDateTimeSeconds }, CancellationToken);
		CheckAlertsAreValid(alerts);
	}

	[Fact]
	public async Task GetFilteredAlertsFor2YearsAgoReturnsNoAlerts()
	{
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _endDateTime.AddYears(-2).SecondsSinceTheEpoch(),
				StartEpochIsBefore = _endDateTime.AddYears(-2).AddDays(2).SecondsSinceTheEpoch()
			}, CancellationToken);
		alerts.Should().BeEmpty();
	}

	[Fact]
	public async Task GetFilteredAlertsForAlertsStartedThisWeekButNotCleared()
	{
		var startUtcIsAfterOrAt = _endDateTime.AddDays(-1);
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartUtcIsAfter = startUtcIsAfterOrAt,
				IsCleared = false,
				IncludeCleared = null
			}, CancellationToken);
		alerts.Should().OnlyContain(a => a.StartOnUtc >= startUtcIsAfterOrAt);
		alerts.Should().OnlyContain(a => !a.IsCleared);
	}

	[Fact]
	public async Task GetFilteredAlertsForAlertsStartedThisWeekAndCleared()
	{
		var startUtcIsAfterOrAt = _endDateTime.AddDays(-7);
		var alerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = startUtcIsAfterOrAt,
			IsCleared = true,
			IncludeCleared = null,
		}, CancellationToken);
		alerts.Should().OnlyContain(a => a.StartOnUtc >= startUtcIsAfterOrAt);
		alerts.Should().OnlyContain(a => a.IsCleared);
	}
}
