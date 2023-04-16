using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Alerts;

public class AlertTests : TestWithOutput
{
	public AlertTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	private const string TestNoteComment = "Test Note";
	private static readonly DateTime EndDateTime = DateTime.UtcNow.Date;
	private static readonly int StartDateTimeSeconds = EndDateTime.AddDays(-7).SecondsSinceTheEpoch();
	private static readonly int EndDateTimeSeconds = EndDateTime.SecondsSinceTheEpoch();

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
			FilterItems = new List<FilterItem<Alert>>
					{
						new Eq<Alert>(nameof(Alert.IsCleared), "*"),
						new Gt<Alert>(nameof(Alert.EndOnSeconds), DaysAgoAsUnixSeconds(10)),
					},
			Order = new Order<Alert>
			{
				Property = nameof(Alert.EndOnSeconds),
				Direction = OrderDirection.Desc
			},
			Take = 300
		};
		var alerts = await LogicMonitorClient.GetAllAsync(closedItemsFilter, default).ConfigureAwait(false);
		alerts.Should().NotBeNull();
		alerts.Should().NotBeEmpty();
		alerts.Should().NotContain(a => a.EndOnSeconds == 0);
	}

	[Fact]
	public async Task AddNoteToAlert()
	{
		var noNoteItemsFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
			{
				new Eq<Alert>(nameof(Alert.AckComment), string.Empty),
			},
			Properties = new List<string>
			{
				nameof(Alert.Id),
				nameof(Alert.InternalId),
			},
			Take = 1
		};
		var alerts = await LogicMonitorClient
			.GetAllAsync(noNoteItemsFilter, default)
			.ConfigureAwait(false);
		alerts.Should().NotBeNull();
		alerts.Should().ContainSingle();

		var alert = alerts[0];
		alert.AckComment.Should().Be("");

		// Add a note
		await LogicMonitorClient
			.SetAlertNoteAsync(new[] { alert.Id }, TestNoteComment, default)
			.ConfigureAwait(false);

		var refetchedAlert = await LogicMonitorClient
			.GetAlertAsync(alert.Id, default)
			.ConfigureAwait(false);

		refetchedAlert.Should().NotBeNull();
		refetchedAlert.AckComment.Should().Be(TestNoteComment);
	}

	[Fact]
	public async Task AlertLevelPropertyIsPopulated()
	{
		var closedItemsFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
					{
						new Eq<Alert>(nameof(Alert.IsCleared), "*"),
						new Gt<Alert>(nameof(Alert.EndOnSeconds), DaysAgoAsUnixSeconds(10)),
					},
			Properties = new List<string>
				{
					nameof(Alert.EndOnSeconds),
					nameof(Alert.Severity)
				},
			Order = new Order<Alert>
			{
				Property = nameof(Alert.EndOnSeconds),
				Direction = OrderDirection.Desc
			},
			Take = 300
		};
		var alerts = await LogicMonitorClient.GetAllAsync(closedItemsFilter, default).ConfigureAwait(false);
		alerts.Should().NotBeNull();
		alerts.Should().NotBeEmpty();
		alerts.Should().NotContain(a => a.EndOnSeconds == 0);
		alerts.Should().NotContain(a => a.AlertLevel == AlertLevel.Unknown);
	}

	[Fact]
	public async Task GetAlerts_SdtsMatchRequest()
	{
		// This unit test currently fails horribly due to some inability of the LogicMonitor API to sort the Alerts by Id descending

		// Arrange
		var allFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.All,
			StartEpochIsAfter = StartDateTimeSeconds,
			StartEpochIsBefore = EndDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};
		var sdtFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.Sdt,
			StartEpochIsAfter = StartDateTimeSeconds,
			StartEpochIsBefore = EndDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};
		var nonSdtFilter = new AlertFilter
		{
			SdtFilter = SdtFilter.NonSdt,
			StartEpochIsAfter = StartDateTimeSeconds,
			StartEpochIsBefore = EndDateTimeSeconds,
			OrderByProperty = nameof(Alert.Id),
			OrderDirection = OrderDirection.Desc
		};

		// Act
		var allAlerts = await LogicMonitorClient.GetAlertsAsync(allFilter, default).ConfigureAwait(false);
		var sdtAlerts = await LogicMonitorClient.GetAlertsAsync(sdtFilter, default).ConfigureAwait(false);
		var nonSdtAlerts = await LogicMonitorClient.GetAlertsAsync(nonSdtFilter, default).ConfigureAwait(false);

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
		Assert.All(sdtAlerts, a => Assert.True(a.InScheduledDownTime));
		Assert.All(nonSdtAlerts, a => Assert.False(a.InScheduledDownTime));
	}

	[Fact]
	public async Task GetAlertsAndCheckUnique()
	{
		var startEpoch = DateTime.UtcNow.AddDays(-1).SecondsSinceTheEpoch();
		var alertList = await LogicMonitorClient.GetRestAlertsWithV84Bug(new AlertFilter { StartEpochIsAfter = startEpoch }, TimeSpan.FromHours(8)).ConfigureAwait(false);

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
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
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
			var alerts = await LogicMonitorClient.GetAlertsAsync(alertFilter, default).ConfigureAwait(false);
			CheckAlertsAreValid(alerts);

			// Make sure there are no alerts for hosts not mentioned by the hostFilter
			Assert.All(alerts, alert => Assert.Equal(alert.MonitorObjectName, device.DisplayName));
			Assert.All(alerts, alert => Assert.Contains(alert.AlertType, alertTypes));
			Assert.All(alerts, alert => Assert.True(alert.StartOnUtc < startUtcIsBefore));
		}
	}

	[Fact]
	public async Task GetAlertsFilteredByDeviceRest()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var deviceGroupFullPathFilter = new List<string> { "Collectors*" };
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
			MonitorObjectGroupFullPaths = deviceGroupFullPathFilter,
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
		var alerts = await LogicMonitorClient.GetAlertsAsync(alertFilter, default).ConfigureAwait(false);
		CheckAlertsAreValid(alerts);

		// Make sure there are no alerts for hosts not mentioned by the hostFilter
		Assert.All(alerts, alert => Assert.Equal(alert.MonitorObjectName, device.DisplayName));
		Assert.True(alerts.All(alert => alert.InstanceName.StartsWith(dataSourceNameFilter, StringComparison.Ordinal)));
		Assert.True(alerts.All(alert => alert.DataPointName.StartsWith(dataPointNameFilter, StringComparison.Ordinal)));
	}

	[Fact]
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
		var alertsNotIncludingCleared = await LogicMonitorClient.GetAlertsAsync(alertFilterNotIncludingCleared, default).ConfigureAwait(false);
		alertsNotIncludingCleared.Should().HaveCountGreaterThan(0);
		Assert.DoesNotContain(alertsNotIncludingCleared, a => !a.IsActive);
		Assert.Contains(alertsNotIncludingCleared, a => a.IsActive);

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
		var alertsIncludngCleared = await LogicMonitorClient.GetAlertsAsync(alertFilterIncludingCleared, default).ConfigureAwait(false);
		Assert.True(alertsIncludngCleared.Count > 0);
		Assert.Contains(alertsIncludngCleared, a => !a.IsActive);
		Assert.Contains(alertsIncludngCleared, a => a.IsActive);
	}

	[Fact]
	public async Task GetFilteredAlerts_LevelsMatch()
	{
		var unfilteredAlerts =
			await LogicMonitorClient
				.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = StartDateTimeSeconds, StartEpochIsBefore = EndDateTimeSeconds }, default)
				.ConfigureAwait(false);
		var criticalAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds,
				Levels = new List<AlertLevel> { AlertLevel.Critical }
			}, default).ConfigureAwait(false);
		var errorAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds,
				Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error }
			}, default).ConfigureAwait(false);
		var warningAlerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds,
				Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
			}, default).ConfigureAwait(false);

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
	public async Task GetFilteredAlertsBrokenLmRep1959()
	{
		var alerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			AckFilter = AckFilter.All,
			StartEpochIsBefore = EndDateTimeSeconds,
			IncludeCleared = false,
			Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error },
			NeedMessage = true,
			OrderDirection = OrderDirection.Desc,
			SdtFilter = SdtFilter.NonSdt,
			StartEpochIsAfter = StartDateTimeSeconds,
			Take = 1
		}, default).ConfigureAwait(false);
		CheckAlertsAreValid(alerts);
	}

	// Time needs to be changed
	[Fact]
	public async Task GetFilteredAlertsDefaultPeriod()
	{
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = StartDateTimeSeconds, StartEpochIsBefore = EndDateTimeSeconds }, default).ConfigureAwait(false);
		CheckAlertsAreValid(alerts);
	}

	[Fact]
	public async Task GetFilteredAlertsFor2YearsAgoReturnsNoAlerts()
	{
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = EndDateTime.AddYears(-2).SecondsSinceTheEpoch(),
				StartEpochIsBefore = EndDateTime.AddYears(-2).AddDays(2).SecondsSinceTheEpoch()
			}, default).ConfigureAwait(false);
		alerts.Should().BeEmpty();
	}

	[Fact]
	public async Task GetFilteredAlertsForAlertsStartedThisWeekButNotCleared()
	{
		var startUtcIsAfterOrAt = EndDateTime.AddDays(-1);
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartUtcIsAfter = startUtcIsAfterOrAt,
				IsCleared = false,
				IncludeCleared = null
			}, default).ConfigureAwait(false);
		Assert.True(alerts.All(a => a.StartOnUtc >= startUtcIsAfterOrAt));
		Assert.True(alerts.All(a => !a.IsCleared));
	}

	[Fact]
	public async Task GetFilteredAlertsForAlertsStartedThisWeekAndCleared()
	{
		var startUtcIsAfterOrAt = EndDateTime.AddDays(-7);
		var alerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = startUtcIsAfterOrAt,
			IsCleared = true,
			IncludeCleared = null,
		}, default).ConfigureAwait(false);
		Assert.True(alerts.All(a => a.StartOnUtc >= startUtcIsAfterOrAt));
		Assert.True(alerts.All(a => a.IsCleared));
	}

	[Fact]
	public async Task GetFilteredAlertsForDatacenter()
	{
		var alerts =
			await LogicMonitorClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds,
				MonitorObjectGroupFullPaths = new List<string> { "Datacenter/*" },
				IncludeCleared = true
			}, default).ConfigureAwait(false);
		CheckAlertsAreValid(alerts);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts.Take(10))
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, default)
				.ConfigureAwait(false);
			refetchedAlert.Should().NotBeNull();
			refetchedAlert.DetailMessage.Should().NotBeNull();
			refetchedAlert.DetailMessage.Subject.Should().NotBeNull();
			refetchedAlert.DetailMessage.Body.Should().NotBeNull();
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.Id.Should().Be(alert.Id);
			refetchedAlert.AlertType.Should().Be(alert.AlertType);
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.DetailMessage?.Body.Should().Be(alert.DetailMessage.Body);
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
				.ConfigureAwait(false);
		};
		await operation
			.Should()
			.ThrowAsync<LogicMonitorApiException>()
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetFilteredAlertsForOneDay()
	{
		var alerts = await LogicMonitorClient.GetAlertsAsync(
			new AlertFilter
			{
				StartEpochIsAfter = DaysAgoAsUnixSeconds(1),
				Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
			}, default).ConfigureAwait(false);
		CheckAlertsAreValid(alerts);
		alerts.Should().NotHaveCount(0);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts)
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, default)
				.ConfigureAwait(false);
			refetchedAlert.Should().NotBeNull();
			refetchedAlert.MonitorObjectId.Should().Be(alert.MonitorObjectId);
			refetchedAlert.Id.Should().Be(alert.Id);
			refetchedAlert.AlertType.Should().Be(alert.AlertType);
			refetchedAlert.DetailMessage?.Body.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetFilteredAlerts_SpecificLevels()
	{
		var timespan = TimeSpan.FromDays(1);

		var allAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = DateTime.UtcNow - timespan,
			Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
		}, default).ConfigureAwait(false);
		CheckAlertsAreValid(allAlerts);

		// Make sure that some are returned with a warning level and some with an error level
		// If not, this test is inconclusive
		if (
			!(allAlerts.Any(a => a.AlertLevel == AlertLevel.Warning)
			  && allAlerts.Any(a => a.AlertLevel == AlertLevel.Error || a.AlertLevel == AlertLevel.Critical)))
		{
			throw new InvalidOperationException($"Inconclusive: Test portal does not have some warning and some error or above alerts in the last {timespan.Humanize()}.");
		}

		// Get filtered alerts, error and above only
		var errorAndAboveAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartUtcIsAfter = DateTime.UtcNow - timespan,
			Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error }
		}, default).ConfigureAwait(false);

		Logger.LogDebug("{ErrorAndAboveCount}", errorAndAboveAlerts.Count);

		// Ensure that the number of error and above is fewer than the total
		Assert.True(errorAndAboveAlerts.Count < allAlerts.Count);

		Assert.True(errorAndAboveAlerts.All(a => a.AlertLevel >= AlertLevel.Error));
	}

	[Fact]
	public async Task GetAlertsMatchingProblemSignature()
	{
		var dateTimeOffset = DateTimeOffset.UtcNow.AddDays(-7);

		var alerts = await LogicMonitorClient.GetAllAsync(new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new FilterItem<Alert> { Property = nameof(Alert.StartOnSeconds), Operation = ">", Value = dateTimeOffset.ToUnixTimeSeconds()},
					new FilterItem<Alert> { Property = nameof(Alert.InternalId), Operation=":", Value="LMS462482416" },
					new FilterItem<Alert> { Property = nameof(Alert.IsCleared), Operation=":", Value="*" },
				}
		}, default).ConfigureAwait(false);
		alerts.Should().NotBeNull();
	}

	[Fact]
	public void LevelDefaultsToError()
	{
		var alertFilter = new AlertFilter();
		alertFilter.Levels.Count.Should().Be(2);
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
		}, default).ConfigureAwait(false);

		// Get all alerts NOT in SDT
		var nonSdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.NonSdt
		}, default).ConfigureAwait(false);
		Assert.All(nonSdtAlerts, alert => Assert.Null(alert.Sdt));

		// Get all alerts in SDT
		var sdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.Sdt
		}, default).ConfigureAwait(false);
		Assert.All(sdtAlerts, alert => alert.Sdt.Should().NotBeNull());

		// Make sure the numbers add up
		(nonSdtAlerts.Count + sdtAlerts.Count).Should().Be(allAlerts.Count);
	}
}
