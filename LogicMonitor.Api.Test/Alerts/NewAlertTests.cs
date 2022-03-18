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
	public async void GetAlerts_SdtsMatchRequest()
	{
		// Arrange
		var allFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
				}
		};
		var sdtFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.InScheduledDownTime), true)
				}
		};
		var nonSdtFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.InScheduledDownTime), false)
				}
		};

		// Act
		var allAlerts = await LogicMonitorClient.GetAllAsync(allFilter).ConfigureAwait(false);
		var sdtAlerts = await LogicMonitorClient.GetAllAsync(sdtFilter).ConfigureAwait(false);
		var nonSdtAlerts = await LogicMonitorClient.GetAllAsync(nonSdtFilter).ConfigureAwait(false);

		// Assert

		// Alert counts add up
		(sdtAlerts.Count + nonSdtAlerts.Count).Should().Be(allAlerts.Count);

		// Alerts have the expected SDT status
		Assert.True(sdtAlerts.All(a => a.InScheduledDownTime));
		Assert.True(nonSdtAlerts.All(a => !a.InScheduledDownTime));
	}

	//[Fact]
	//public async void GetAlerts_AckMatchesRequest()
	//{
	//	// Arrange
	//	var allFilter = new Filter<Alert>
	//	{
	//		FilterItems = new List<FilterItem<Alert>>
	//		{
	//			new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
	//			new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
	//		}
	//	};
	//	var ackedFilter = new Filter<Alert>
	//	{
	//		FilterItems = new List<FilterItem<Alert>>
	//		{
	//			new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
	//			new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
	//			new Eq<Alert>(nameof(Alert.Acked), true)
	//		}
	//	};
	//	var nonAckedFilter = new Filter<Alert>
	//	{
	//		FilterItems = new List<FilterItem<Alert>>
	//		{
	//			new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
	//			new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
	//			new Eq<Alert>(nameof(Alert.Acked), false)
	//		}
	//	};

	//	// Act
	//	var allAlerts = await DefaultPortalClient.GetAllAsync(allFilter).ConfigureAwait(false);
	//	var ackedAlerts = await DefaultPortalClient.GetAllAsync(ackedFilter).ConfigureAwait(false);
	//	var nonAckedAlerts = await DefaultPortalClient.GetAllAsync(nonAckedFilter).ConfigureAwait(false);

	//	// Assert
	//	var expectedTotalAlertIds = ackedAlerts.Union(nonAckedAlerts).Select(alert => alert.Id).ToList();
	//	var inAllButNotInExpected = allAlerts.Where(a => !expectedTotalAlertIds.Contains(a.Id));

	//	// Alert counts add up
	//	ackedAlerts.Count + nonAckedAlerts.Count.Should().Be(allAlerts.Count);

	//	// Alerts have the expected Acked status
	//	// NOTE This could be different if the AckedNote is blank, this seems to NOT bucket that alert into nonAckedAlerts
	//	Assert.True(ackedAlerts.All(a => a.Acked));
	//	Assert.True(nonAckedAlerts.All(a => !a.Acked));
	//}

	[Fact]
	public async void GetAlertsAndCheckUnique()
	{
		var allFilter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
				}
		};

		var allAlerts = await LogicMonitorClient.GetAllAsync(allFilter).ConfigureAwait(false);

		var unique = true;
		foreach (var alert in allAlerts)
		{
			if (allAlerts.Count(a => a.Id == alert.Id) > 1)
			{
				unique = false;
			}
		}

		Assert.True(unique);
	}

	[Fact]
	public async void GetAlertsFilteredByDevice()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		foreach (var alertType in new List<AlertType> { AlertType.DataSource, AlertType.EventSource })
		{
			var alertFilter = new Filter<Alert>
			{
				Skip = 0,
				Take = 10,
				FilterItems = new List<FilterItem<Alert>>
					{
						new Eq<Alert>(nameof(Alert.MonitorObjectName), device.DisplayName)
					}
			};

			// Refetch each alert
			foreach (var alert in await LogicMonitorClient.GetAllAsync(alertFilter).ConfigureAwait(false))
			{
				var a = await LogicMonitorClient
					.GetAlertAsync(alert.Id)
					.ConfigureAwait(false);
				a.DetailMessage.Should().NotBeNull();
			}
		}
	}

	[Fact]
	public async void MultipleLevels()
	{
		var severities = new List<int> { 4, 2 };
		var filter = new Filter<Alert>
		{
			FilterItems = new List<FilterItem<Alert>>
				{
					new Gt<Alert>(nameof(Alert.StartOnSeconds), StartDateTimeSeconds),
					new Lt<Alert>(nameof(Alert.StartOnSeconds), EndDateTimeSeconds),
					new Eq<Alert>(nameof(Alert.Severity), severities)
				}
		};

		// Act
		var alerts = await LogicMonitorClient.GetAllAsync(filter).ConfigureAwait(false);
		Assert.All(alerts, alert => severities.Contains(alert.Severity));
	}

	[Fact]
	public async void GetNoExistentAlertAsJObject()
	{
		var result = await LogicMonitorClient
					.GetJObjectAsync("alert/alerts/DS_NonExistent", CancellationToken.None)
					.ConfigureAwait(false);
		result.Should().BeNull();
	}
}
