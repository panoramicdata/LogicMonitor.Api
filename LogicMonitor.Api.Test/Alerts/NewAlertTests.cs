namespace LogicMonitor.Api.Test.Alerts;

public class NewAlertTests : TestWithOutput
{
	public NewAlertTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	private static readonly DateTime EndDateTime = DateTime.UtcNow.Date;
	private static readonly int StartDateTimeSeconds = EndDateTime.AddDays(-14).SecondsSinceTheEpoch();
	private static readonly int EndDateTimeSeconds = EndDateTime.SecondsSinceTheEpoch();

	[Fact]
	public async Task GetAlerts_SdtsMatchRequest()
	{
		// Arrange
		var allFilter = new Filter<Alert>
		{
			FilterItems =
				[
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
				]
		};
		var sdtFilter = new Filter<Alert>
		{
			FilterItems =
				[
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.InScheduledDownTime), true)
				]
		};
		var nonSdtFilter = new Filter<Alert>
		{
			FilterItems =
				[
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.InScheduledDownTime), false)
				]
		};

		// Act
		var allAlerts = await LogicMonitorClient.GetAllAsync(allFilter, default).ConfigureAwait(false);
		var sdtAlerts = await LogicMonitorClient.GetAllAsync(sdtFilter, default).ConfigureAwait(false);
		var nonSdtAlerts = await LogicMonitorClient.GetAllAsync(nonSdtFilter, default).ConfigureAwait(false);

		// Assert

		// Alert counts add up
		(sdtAlerts.Count + nonSdtAlerts.Count).Should().Be(allAlerts.Count);

		// Alerts have the expected SDT status
		Assert.All(sdtAlerts, a => Assert.True(a.InScheduledDownTime));
		Assert.All(nonSdtAlerts, a => Assert.False(a.InScheduledDownTime));
	}

	[Fact]
	public async Task GetAlertsAndCheckUnique()
	{
		var allFilter = new Filter<Alert>
		{
			FilterItems =
				[
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
				]
		};

		var allAlerts = await LogicMonitorClient.GetAllAsync(allFilter, default).ConfigureAwait(false);

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
	public async Task GetAlertsFilteredByDevice()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
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
			foreach (var alert in await LogicMonitorClient.GetAllAsync(alertFilter, default).ConfigureAwait(false))
			{
				var a = await LogicMonitorClient
					.GetAlertAsync(alert.Id, default)
					.ConfigureAwait(false);
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
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.Severity), severities)
				]
		};

		// Act
		var alerts = await LogicMonitorClient.GetAllAsync(filter, default).ConfigureAwait(false);
		alerts.Should().AllSatisfy(alert => severities.Contains(alert.Severity).Should().BeTrue());
	}

	[Fact]
	public async Task GetNonExistentAlertAsJObject()
	{
		var action = async () => await LogicMonitorClient
					.GetJObjectAsync("alert/alerts/DS_NonExistent", default)
					.ConfigureAwait(false);
		await action
			.Should()
			.ThrowAsync<LogicMonitorApiException>()
			.ConfigureAwait(false);
	}
}
