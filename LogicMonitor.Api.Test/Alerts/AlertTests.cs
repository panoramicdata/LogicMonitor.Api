using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Alerts;

public class AlertTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private const string TestNoteComment = "Test Note";
	private static readonly DateTime _endDateTime = DateTime.UtcNow.Date;
	private static readonly int _startDateTimeSeconds = _endDateTime.AddDays(-7).SecondsSinceTheEpoch();
	private static readonly int _endDateTimeSeconds = _endDateTime.SecondsSinceTheEpoch();

	private static void CheckAlertsAreValid(List<Alert> alerts)
	{
		// Make sure that all have Unique Ids
		foreach (var alertType in Enum.GetValues(typeof(AlertType)).Cast<AlertType>())
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
			.GetAllAsync(closedItemsFilter, default)
			.ConfigureAwait(true);
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
			.GetAllAsync(noNoteItemsFilter, default)
			.ConfigureAwait(true);
		alerts.Should().NotBeNull();
		alerts.Should().ContainSingle();

		var alert = alerts[0];
		alert.AckComment.Should().Be("");

		// Add a note
		await LogicMonitorClient
			.SetAlertNoteAsync([alert.Id], TestNoteComment, default)
			.ConfigureAwait(true);

		var refetchedAlert = await LogicMonitorClient
			.GetAlertAsync(alert.Id, default)
			.ConfigureAwait(true);

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
			.GetAllAsync(closedItemsFilter, default)
			.ConfigureAwait(true);
		alerts.Should().NotBeNull();
		alerts.Should().NotBeEmpty();
		alerts.Should().NotContain(a => a.EndOnSeconds == 0);
		alerts.Should().NotContain(a => a.AlertLevel == AlertLevel.Unknown);
	}

	[Fact]
	[Trait("Long Tests", "")]
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
			.GetAlertsAsync(allFilter, default)
			.ConfigureAwait(true);
		var sdtAlerts = await LogicMonitorClient
			.GetAlertsAsync(sdtFilter, default)
			.ConfigureAwait(true);
		var nonSdtAlerts = await LogicMonitorClient
			.GetAlertsAsync(nonSdtFilter, default)
			.ConfigureAwait(true);

		// Assert

		// All the ids should be unique
		allAlerts.Should().OnlyHaveUniqueItems(a => a.Id);
		sdtAlerts.Should().OnlyHaveUniqueItems(a => a.Id);
		nonSdtAlerts.Should().OnlyHaveUniqueItems(a => a.Id);

		// Troubleshooting stuff
		var extraNonSdtAlertIds = nonSdtAlerts.Select(a => a.Id).Except(allAlerts.Select(a => a.Id)).ToList();
		var extraAllAlertIds = allAlerts.Select(a => a.Id).Except(nonSdtAlerts.Select(a => a.Id)).ToList();

		var extraNonSdtAlerts = nonSdtAlerts.Where(a => extraNonSdtAlertIds.Contains(a.Id)).ToList();
		var extraAllSdtAlerts = allAlerts.Where(a => extraAllAlertIds.Contains(a.Id)).ToList();

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
			.GetRestAlertsWithV84BugAsync(new AlertFilter { StartEpochIsAfter = startEpoch }, TimeSpan.FromHours(8))
			.ConfigureAwait(true);

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
	public async Task GetAlertsFilteredByDevice()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
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
				.GetAlertsAsync(alertFilter, default)
				.ConfigureAwait(true);
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
	public async Task GetAlertsFilteredByDeviceRest()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
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
			.GetAlertsAsync(alertFilter, default)
			.ConfigureAwait(true);
		CheckAlertsAreValid(alerts);

		// Make sure there are no alerts for hosts not mentioned by the hostFilter
		alerts.Should().AllSatisfy(alert => device.DisplayName.Should().Be(alert.MonitorObjectName));
		alerts.Should().OnlyContain(alert => alert.InstanceName.StartsWith(dataSourceNameFilter, StringComparison.Ordinal));
		alerts.Should().OnlyContain(alert => alert.DataPointName.StartsWith(dataPointNameFilter, StringComparison.Ordinal));
	}

	[Fact]
	[Trait("Long Tests", "")]
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
			.GetAlertsAsync(alertFilterNotIncludingCleared, default)
			.ConfigureAwait(true);
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
			.GetAlertsAsync(alertFilterIncludingCleared, default)
			.ConfigureAwait(true);
		alertsIncludingCleared.Count.Should().BePositive();
		alertsIncludingCleared.Should().Contain(a => !a.IsActive);
		alertsIncludingCleared.Should().Contain(a => a.IsActive);
	}

	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetFilteredAlerts_LevelsMatch()
	{
		var unfilteredAlerts =
			await LogicMonitorClient
				.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = _startDateTimeSeconds, StartEpochIsBefore = _endDateTimeSeconds }, default)
				.ConfigureAwait(true);
		var criticalAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical]
			}, default).ConfigureAwait(true);
		var errorAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical, AlertLevel.Error]
			}, default).ConfigureAwait(true);
		var warningAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				Levels = [AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning]
			}, default).ConfigureAwait(true);

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
		}, default).ConfigureAwait(true);
		CheckAlertsAreValid(alerts);
	}

	// Time needs to be changed
	[Fact]
	public async Task GetFilteredAlertsDefaultPeriod()
	{
		var alerts =
			await LogicMonitorClient
				.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = _startDateTimeSeconds, StartEpochIsBefore = _endDateTimeSeconds }, default)
				.ConfigureAwait(true);
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
			}, default).ConfigureAwait(true);
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
			}, default).ConfigureAwait(true);
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
		}, default).ConfigureAwait(true);
		alerts.Should().OnlyContain(a => a.StartOnUtc >= startUtcIsAfterOrAt);
		alerts.Should().OnlyContain(a => a.IsCleared);
	}

	[Fact]
	public async Task GetFilteredAlertsForDatacenter()
	{
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = _startDateTimeSeconds,
				StartEpochIsBefore = _endDateTimeSeconds,
				MonitorObjectGroupFullPaths = ["Datacenter/*"],
				IncludeCleared = true
			}, default).ConfigureAwait(true);
		CheckAlertsAreValid(alerts);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts.Take(10))
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, default)
				.ConfigureAwait(true);
			refetchedAlert.Should().NotBeNull();
			refetchedAlert.DetailMessage.Should().NotBeNull();
			refetchedAlert.DetailMessage.Subject.Should().NotBeNull();
			refetchedAlert.DetailMessage.Body.Should().NotBeNull();
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.Id.Should().Be(alert.Id);
			refetchedAlert.AlertType.Should().Be(alert.AlertType);
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.DetailMessage.Body.Should().Be(alert.DetailMessage.Body);
		}
	}

	[Fact]
	public async Task GetOfNonExistentAlertShouldThrowException()
	{
		Alert alert;
		var operation = async () =>
		{
			alert = await LogicMonitorClient
				.GetAlertAsync("DS1234", default)
				.ConfigureAwait(true);
		};
		await operation
			.Should()
			.ThrowAsync<LogicMonitorApiException>()
			.ConfigureAwait(true);
	}

	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetFilteredAlertsForOneDay()
	{
		var alerts = await LogicMonitorClient.GetAlertsAsync(
			new AlertFilter
			{
				StartEpochIsAfter = DaysAgoAsUnixSeconds(1),
				Levels = [AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning]
			}, default).ConfigureAwait(true);
		CheckAlertsAreValid(alerts);
		alerts.Should().NotHaveCount(0);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts)
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, default)
				.ConfigureAwait(true);
			refetchedAlert.Should().NotBeNull();
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.Id.Should().Be(alert.Id);
			refetchedAlert.AlertType.Should().Be(alert.AlertType);
			refetchedAlert.DetailMessage.Body.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetFilteredAlerts_SpecificLevels()
	{
		var timespan = TimeSpan.FromDays(1);

		var allAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = DateTime.UtcNow - timespan,
			Levels = [AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning]
		}, default).ConfigureAwait(true);
		CheckAlertsAreValid(allAlerts);

		// Make sure that some are returned with a warning level and some with an error level
		// If not, this test is inconclusive
		if (
			!(allAlerts.Any(a => a.AlertLevel == AlertLevel.Warning)
			  && allAlerts.Any(a => a.AlertLevel is AlertLevel.Error or AlertLevel.Critical)))
		{
			throw new InvalidOperationException($"Inconclusive: Test portal does not have some warning and some error or above alerts in the last {timespan.Humanize()}.");
		}

		// Get filtered alerts, error and above only
		var errorAndAboveAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = DateTime.UtcNow - timespan,
			Levels = [AlertLevel.Critical, AlertLevel.Error]
		}, default).ConfigureAwait(true);

		Logger.LogDebug("{ErrorAndAboveCount}", errorAndAboveAlerts.Count);

		// Ensure that the number of error and above is fewer than the total
		(errorAndAboveAlerts.Count < allAlerts.Count).Should().BeTrue();

		errorAndAboveAlerts.Should().OnlyContain(a => a.AlertLevel >= AlertLevel.Error);
	}

	[Fact]
	public async Task GetAlertsMatchingProblemSignature()
	{
		var dateTimeOffset = DateTimeOffset.UtcNow.AddDays(-7);

		var alerts = await LogicMonitorClient.GetAllAsync(new Filter<Alert>
		{
			FilterItems =
			[
				new FilterItem<Alert> { Property = nameof(Alert.StartOnSeconds), Operation = ">", Value = dateTimeOffset.ToUnixTimeSeconds() },
				new FilterItem<Alert> { Property = nameof(Alert.InternalId), Operation = ":", Value = "LMS462482416" },
				new FilterItem<Alert> { Property = nameof(Alert.IsCleared), Operation = ":", Value = "*" },
			]
		}, default).ConfigureAwait(true);
		alerts.Should().NotBeNull();
	}

	[Fact]
	public void LevelDefaultsToError()
	{
		var alertFilter = new AlertFilter();
		alertFilter.Levels.Should().HaveCount(2);
		alertFilter.Levels[0].Should().Be(AlertLevel.Error);
		alertFilter.Levels[1].Should().Be(AlertLevel.Critical);
	}

	[Fact]
	public async Task SdtFilter_Works()
	{
		var nowUtc = DateTimeOffset.UtcNow;
		var oneDayAgo = nowUtc.AddDays(-1);

		// Get all alerts
		var allAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
		}, default).ConfigureAwait(true);

		// Get all alerts NOT in SDT
		var nonSdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.NonSdt
		}, default).ConfigureAwait(true);
		nonSdtAlerts.Should().AllSatisfy(alert => alert.Sdt.Should().BeNull());

		// Get all alerts in SDT
		var sdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.Sdt
		}, default).ConfigureAwait(true);
		sdtAlerts.Should().AllSatisfy(alert => alert.Sdt.Should().NotBeNull());

		// Make sure the numbers add up
		(nonSdtAlerts.Count + sdtAlerts.Count).Should().Be(allAlerts.Count);
	}

	[Fact]
	public async Task AckAlert()
	{
		var notAckedFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Eq<Alert>(nameof(Alert.Acked), false),
			],
			Take = 1
		};

		var nonAckedAlerts = await LogicMonitorClient
			.GetAllAsync(notAckedFilter, default)
			.ConfigureAwait(true);

		var alertToAck = nonAckedAlerts[0];
		var alertId = alertToAck.Id;

		await LogicMonitorClient
			.AcknowledgeAlertAsync(alertId, "acknowledgementTest", default)
			.ConfigureAwait(true);

		var fetchAlert = await LogicMonitorClient
			.GetAlertAsync(alertId, default)
			.ConfigureAwait(true);

		fetchAlert.Acked.Should().Be(true);
	}
}
