namespace LogicMonitor.Api.Test.Alerts;

public class NewAlertTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private static readonly DateTime _endDateTime = DateTime.UtcNow.Date;
	private static readonly int _startDateTimeSeconds = _endDateTime.AddDays(-14).SecondsSinceTheEpoch();
	private static readonly int _endDateTimeSeconds = _endDateTime.SecondsSinceTheEpoch();

	[Fact]
	public async Task GetAlerts_SdtsMatchRequest()
	{
		// Arrange
		var allFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Gt<Alert>(nameof(Alert.StartOnSeconds), _startDateTimeSeconds),
				new Lt<Alert>(nameof(Alert.StartOnSeconds), _endDateTimeSeconds),
			]
		};
		var sdtFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Gt<Alert>(nameof(Alert.StartOnSeconds), _startDateTimeSeconds),
				new Lt<Alert>(nameof(Alert.StartOnSeconds), _endDateTimeSeconds),
				new Eq<Alert>(nameof(Alert.InScheduledDownTime), true)
			]
		};
		var nonSdtFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Gt<Alert>(nameof(Alert.StartOnSeconds), _startDateTimeSeconds),
				new Lt<Alert>(nameof(Alert.StartOnSeconds), _endDateTimeSeconds),
				new Eq<Alert>(nameof(Alert.InScheduledDownTime), false)
			]
		};

		// Act
		var allAlerts = await LogicMonitorClient
			.GetAllAsync(allFilter, default)
			.ConfigureAwait(true);
		var sdtAlerts = await LogicMonitorClient
			.GetAllAsync(sdtFilter, default)
			.ConfigureAwait(true);
		var nonSdtAlerts = await LogicMonitorClient
			.GetAllAsync(nonSdtFilter, default)
			.ConfigureAwait(true);

		// Assert

		// Alert counts add up
		(sdtAlerts.Count + nonSdtAlerts.Count).Should().Be(allAlerts.Count);

		// Alerts have the expected SDT status
		sdtAlerts.Should().AllSatisfy(a => a.InScheduledDownTime.Should().BeTrue());
		nonSdtAlerts.Should().AllSatisfy(a => a.InScheduledDownTime.Should().BeFalse());
	}

	[Fact]
	public async Task GetAlertsAndCheckUnique()
	{
		var allFilter = new Filter<Alert>
		{
			FilterItems =
			[
				new Gt<Alert>(nameof(Alert.StartOnSeconds), _startDateTimeSeconds),
				new Lt<Alert>(nameof(Alert.StartOnSeconds), _endDateTimeSeconds),
			]
		};

		var allAlerts = await LogicMonitorClient
			.GetAllAsync(allFilter, default)
			.ConfigureAwait(true);

		var unique = true;
		foreach (var alert in allAlerts)
		{
			if (allAlerts.Count(a => a.Id == alert.Id) > 1)
			{
				unique = false;
			}
		}

		unique.Should().BeTrue();
	}

	[Fact]
	public async Task GetAlertsFilteredByResource()
	{
		var device = await GetWindowsResourceAsync(default)
			.ConfigureAwait(true);
		foreach (var alertType in new List<AlertType> { AlertType.DataSource, AlertType.EventSource })
		{
			var alertFilter = new Filter<Alert>
			{
				Skip = 0,
				Take = 10,
				FilterItems =
				[
					new Eq<Alert>(nameof(Alert.MonitorObjectName), device.DisplayName)
				]
			};

			// Refetch each alert
			foreach (var alert in await LogicMonitorClient.GetAllAsync(alertFilter, default).ConfigureAwait(true))
			{
				var a = await LogicMonitorClient
					.GetAlertAsync(alert.Id, default)
					.ConfigureAwait(true);
				a.DetailMessage.Should().NotBeNull();
			}
		}
	}

	[Fact]
	public async Task MultipleLevels()
	{
		var severities = new List<int> { 4, 2 };
		var filter = new Filter<Alert>
		{
			FilterItems =
			[
				new Gt<Alert>(nameof(Alert.StartOnSeconds), _startDateTimeSeconds),
				new Lt<Alert>(nameof(Alert.StartOnSeconds), _endDateTimeSeconds),
				new Eq<Alert>(nameof(Alert.Severity), severities)
			]
		};

		// Act
		var alerts = await LogicMonitorClient
			.GetAllAsync(filter, default)
			.ConfigureAwait(true);
		alerts.Should().AllSatisfy(alert => severities.Should().Contain(alert.Severity));
	}

	[Fact]
	public async Task GetNonExistentAlertAsJObject()
	{
		var action = async () => await LogicMonitorClient
			.GetJObjectAsync("alert/alerts/DS_NonExistent", default)
			.ConfigureAwait(true);
		await action
			.Should()
			.ThrowAsync<LogicMonitorApiException>()
			.ConfigureAwait(true);
	}
}
