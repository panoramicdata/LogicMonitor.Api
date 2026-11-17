using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Alerts;

public partial class AlertTests
{

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
			}, CancellationToken);
		CheckAlertsAreValid(alerts);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts.Take(10))
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, CancellationToken);
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
	public Task GetOfNonExistentAlertShouldThrowException()
			=> ((Func<Task>?)(async () =>
				{
					_ = await LogicMonitorClient.GetAlertAsync("DS1234", CancellationToken);
				}))
				.Should()
				.ThrowAsync<LogicMonitorApiException>();

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetFilteredAlertsForOneDay()
	{
		var alerts = await LogicMonitorClient.GetAlertsAsync(
			new AlertFilter
			{
				StartEpochIsAfter = DaysAgoAsUnixSeconds(1),
				Levels = [AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning]
			}, CancellationToken);
		CheckAlertsAreValid(alerts);
		alerts.Should().NotHaveCount(0);

		// Refetch with GetAlertAsync
		foreach (var alert in alerts)
		{
			var refetchedAlert = await LogicMonitorClient
				.GetAlertAsync(alert.Id, CancellationToken);
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
		}, CancellationToken);
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
		}, CancellationToken);

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
		}, CancellationToken);
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
		}, CancellationToken);

		// Get all alerts NOT in SDT
		var nonSdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.NonSdt
		}, CancellationToken);
		nonSdtAlerts.Should().AllSatisfy(alert => alert.Sdt.Should().BeNull());

		// Get all alerts in SDT
		var sdtAlerts = await LogicMonitorClient.GetAlertsAsync(new AlertFilter
		{
			StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
			StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			SdtFilter = SdtFilter.Sdt
		}, CancellationToken);
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
			.GetAllAsync(notAckedFilter, CancellationToken);

		var alertToAck = nonAckedAlerts[0];
		var alertId = alertToAck.Id;

		await LogicMonitorClient
			.AcknowledgeAlertAsync(alertId, "acknowledgementTest", CancellationToken);

		// Allow time for the acknowledgement to propagate
		Alert? fetchAlert = null;
		for (var attempt = 0; attempt < 5; attempt++)
		{
			await Task.Delay(1000, CancellationToken);
			fetchAlert = await LogicMonitorClient
				.GetAlertAsync(alertId, CancellationToken);
			if (fetchAlert.Acked)
			{
				break;
			}
		}

		fetchAlert.Should().NotBeNull();
		fetchAlert!.Acked.Should().Be(true);
	}

	[Fact]
	public void LastUpdatedOnMs_ConvertsCorrectlyToDateTime()
	{
		// Arrange - Create an alert with a specific timestamp in milliseconds
		// 1746002725588 ms since epoch = 2025-04-30 08:45:25.588 UTC
		var alert = new Alert
		{
			LastUpdatedOnMs = 1746002725588
		};

		// Act
		var lastUpdatedUtc = alert.LastUpdatedOnUtc;

		// Assert
		lastUpdatedUtc.Should().NotBeNull();
		lastUpdatedUtc.Should().HaveYear(2025);
		lastUpdatedUtc.Should().HaveMonth(4);
		lastUpdatedUtc.Should().HaveDay(30);
		lastUpdatedUtc.Should().HaveHour(8);
		lastUpdatedUtc.Should().HaveMinute(45);
		lastUpdatedUtc.Should().HaveSecond(25);
	
		// Verify the exact timestamp
		var expectedDateTime = new DateTime(2025, 4, 30, 8, 45, 25, 588, DateTimeKind.Utc);
		lastUpdatedUtc.Should().Be(expectedDateTime);
	}

	[Fact]
	public void LastUpdatedOnMs_WhenNull_ReturnsNull()
	{
		// Arrange
		var alert = new Alert
		{
			LastUpdatedOnMs = null
		};

		// Act
		var lastUpdatedUtc = alert.LastUpdatedOnUtc;

		// Assert
		lastUpdatedUtc.Should().BeNull();
	}
}
