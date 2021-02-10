using Humanizer;
using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Alerts
{
	public class AlertTests : TestWithOutput
	{
		public AlertTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		private static readonly DateTime EndDateTime = DateTime.UtcNow.Date;
		private static readonly int StartDateTimeSeconds = EndDateTime.AddDays(-7).SecondsSinceTheEpoch();
		private static readonly int EndDateTimeSeconds = EndDateTime.SecondsSinceTheEpoch();

		private static void CheckAlertsAreValid(List<Alert> alerts)
		{
			// Make sure that all have Unique Ids
			foreach (var alertType in Enum.GetValues(typeof(AlertType)).Cast<AlertType>())
			{
				Assert.False(alerts.Where(a => a.AlertType == alertType).Select(a => a.Id).HasDuplicates());
			}
		}

		[Fact]
		public async void ClosedAlerts()
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
			var alerts = await PortalClient.GetAllAsync(closedItemsFilter).ConfigureAwait(false);
			Assert.NotNull(alerts);
			Assert.NotEmpty(alerts);
			Assert.DoesNotContain(alerts, a => a.EndOnSeconds == 0);
			// var closedAlerts = alerts.Where(a => a.EndOnSeconds > 0).ToList();
		}

		[Fact]
		public async void AlertLevelPropertyIsPopulated()
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
			var alerts = await PortalClient.GetAllAsync(closedItemsFilter).ConfigureAwait(false);
			Assert.NotNull(alerts);
			Assert.NotEmpty(alerts);
			Assert.DoesNotContain(alerts, a => a.EndOnSeconds == 0);
			Assert.All(alerts, a => Assert.NotEqual(AlertLevel.Unknown,a.AlertLevel));
		}

		[Fact]
		public async void GetAlerts_SdtsMatchRequest()
		{
			// Arrange
			var allFilter = new AlertFilter
			{
				SdtFilter = SdtFilter.All,
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds
			};
			var sdtFilter = new AlertFilter
			{
				SdtFilter = SdtFilter.Sdt,
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds
			};
			var nonSdtFilter = new AlertFilter
			{
				SdtFilter = SdtFilter.NonSdt,
				StartEpochIsAfter = StartDateTimeSeconds,
				StartEpochIsBefore = EndDateTimeSeconds
			};

			// Act
			var allAlerts = await PortalClient.GetAlertsAsync(allFilter).ConfigureAwait(false);
			var sdtAlerts = await PortalClient.GetAlertsAsync(sdtFilter).ConfigureAwait(false);
			var nonSdtAlerts = await PortalClient.GetAlertsAsync(nonSdtFilter).ConfigureAwait(false);

			// Assert

			// Alert counts add up
			Assert.Equal(allAlerts.Count, sdtAlerts.Count + nonSdtAlerts.Count);

			// Alerts have the expected SDT status
			Assert.True(sdtAlerts.All(a => a.InScheduledDownTime));
			Assert.True(nonSdtAlerts.All(a => !a.InScheduledDownTime));
		}

		//[Fact]
		//public async void GetAlerts_AckMatchesRequest()
		//{
		//	// Arrange
		//	var allFilter = new AlertFilter
		//	{
		//		AckFilter = AckFilter.All,
		//		StartEpochIsAfter = StartDateTimeSeconds,
		//		StartEpochIsBefore = EndDateTimeSeconds
		//	};
		//	var ackFilter = new AlertFilter
		//	{
		//		AckFilter = AckFilter.Acked,
		//		StartEpochIsAfter = StartDateTimeSeconds,
		//		StartEpochIsBefore = EndDateTimeSeconds
		//	};
		//	var nonAckFilter = new AlertFilter
		//	{
		//		AckFilter = AckFilter.Nonacked,
		//		StartEpochIsAfter = StartDateTimeSeconds,
		//		StartEpochIsBefore = EndDateTimeSeconds
		//	};

		//	// Act
		//	var allAlerts = await DefaultPortalClient.GetAlertsAsync(allFilter).ConfigureAwait(false);
		//	var ackAlerts = await DefaultPortalClient.GetAlertsAsync(ackFilter).ConfigureAwait(false);
		//	var nonAckAlerts = await DefaultPortalClient.GetAlertsAsync(nonAckFilter).ConfigureAwait(false);

		//	// Assert

		//	// Alert counts add up
		//	Assert.Equal(allAlerts.Count, ackAlerts.Count + nonAckAlerts.Count);

		//	// Alerts have the expected Ack status
		//	Assert.True(ackAlerts.All(a => a.Acked));
		//	Assert.True(nonAckAlerts.All(a => !a.Acked));
		//}

		[Fact]
		public async void GetAlertsAndCheckUnique()
		{
			var startEpoch = DateTime.UtcNow.AddDays(-1).SecondsSinceTheEpoch();
			var alertList = await PortalClient.GetRestAlertsWithV84Bug(new AlertFilter { StartEpochIsAfter = startEpoch }, TimeSpan.FromHours(8)).ConfigureAwait(false);

			var unique = true;
			foreach (var alert in alertList)
			{
				if (alertList.Count(a => a.Id == alert.Id) > 1)
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
				var alerts = await PortalClient.GetAlertsAsync(alertFilter).ConfigureAwait(false);
				// TODO CheckAlertsAreValid(alerts);

				// Make sure there are no alerts for hosts not mentioned by the hostFilter
				Assert.DoesNotContain(alerts, alert => alert.MonitorObjectName != device.DisplayName);
				Assert.All(alerts, alert => Assert.Contains(alert.AlertType, alertTypes));
				Assert.All(alerts, alert => Assert.True(alert.StartOnUtc < startUtcIsBefore));
			}
		}

		[Fact]
		public async void GetAlertsFilteredByDeviceRest()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
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
			var alerts = await PortalClient.GetAlertsAsync(alertFilter).ConfigureAwait(false);
			// TODO CheckAlertsAreValid(alerts);

			// Make sure there are no alerts for hosts not mentioned by the hostFilter
			Assert.True(alerts.All(alert => alert.MonitorObjectName == device.DisplayName));
			//Assert.True(alerts.All(alert => alert.MonitorObjectGroups.Any(mog => mog.FullPath.StartsWith(deviceGroupFullPathFilter.Replace("*", "")))));
			Assert.True(alerts.All(alert => alert.InstanceName.StartsWith(dataSourceNameFilter)));
			Assert.True(alerts.All(alert => alert.DataPointName.StartsWith(dataPointNameFilter)));
		}

		[Fact]
		public async void GetAlertsIncludeInactiveShouldWork()
		{
			// IncludeCleared set false should bring back only active alerts
			var alertFilterNotIncludingCleared = new AlertFilter
			{
				IncludeCleared = false,
				AckFilter = AckFilter.All,
				AlertRuleName = null,
				StartEpochIsBefore = DateTime.UtcNow.AddMinutes(-1).SecondsSinceTheEpoch(),
				SdtFilter = SdtFilter.All,
				StartEpochIsAfter = DateTime.UtcNow.AddDays(-7).SecondsSinceTheEpoch(),
				OrderByProperty = nameof(Alert.StartOnSeconds),
				OrderDirection = OrderDirection.Desc
			};
			var alertsNotIncludingCleared = await PortalClient.GetAlertsAsync(alertFilterNotIncludingCleared).ConfigureAwait(false);
			Assert.True(alertsNotIncludingCleared.Count > 0);
			Assert.DoesNotContain(alertsNotIncludingCleared, a => !a.IsActive);
			Assert.Contains(alertsNotIncludingCleared, a => a.IsActive);

			// IncludeCleared set true should bring back active AND non-active alerts
			var alertFilterIncludingCleared = new AlertFilter
			{
				IncludeCleared = true,
				AckFilter = AckFilter.All,
				AlertRuleName = null,
				StartEpochIsBefore = DateTime.UtcNow.AddMinutes(-1).SecondsSinceTheEpoch(),
				SdtFilter = SdtFilter.All,
				StartEpochIsAfter = DateTime.UtcNow.AddDays(-7).SecondsSinceTheEpoch(),
				OrderByProperty = nameof(Alert.StartOnSeconds),
				OrderDirection = OrderDirection.Desc
			};
			var alertsIncludngCleared = await PortalClient.GetAlertsAsync(alertFilterIncludingCleared).ConfigureAwait(false);
			Assert.True(alertsIncludngCleared.Count > 0);
			Assert.Contains(alertsIncludngCleared, a => !a.IsActive);
			Assert.Contains(alertsIncludngCleared, a => a.IsActive);
		}

		[Fact]
		public async void GetFilteredAlerts_LevelsMatch()
		{
			var unfilteredAlerts =
				await PortalClient.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = StartDateTimeSeconds, StartEpochIsBefore = EndDateTimeSeconds }).ConfigureAwait(false);
			var criticalAlerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartEpochIsAfter = StartDateTimeSeconds,
					StartEpochIsBefore = EndDateTimeSeconds,
					Levels = new List<AlertLevel> { AlertLevel.Critical }
				}).ConfigureAwait(false);
			var errorAlerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartEpochIsAfter = StartDateTimeSeconds,
					StartEpochIsBefore = EndDateTimeSeconds,
					Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error }
				}).ConfigureAwait(false);
			var warningAlerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartEpochIsAfter = StartDateTimeSeconds,
					StartEpochIsBefore = EndDateTimeSeconds,
					Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
				}).ConfigureAwait(false);

			// Ensure that all alerts are at the appropriate level
			Assert.True(unfilteredAlerts.All(a => a.AlertLevel >= AlertLevel.Error));
			Assert.True(criticalAlerts.All(a => a.AlertLevel >= AlertLevel.Critical));
			Assert.True(errorAlerts.All(a => a.AlertLevel >= AlertLevel.Error));
			Assert.True(warningAlerts.All(a => a.AlertLevel >= AlertLevel.Warning));

			// Ensure that the counts make sense
			Assert.True(unfilteredAlerts.Count == errorAlerts.Count);
			Assert.True(criticalAlerts.Count <= errorAlerts.Count);
			Assert.True(errorAlerts.Count <= warningAlerts.Count);
		}

		[Fact]
		public async void GetFilteredAlertsBrokenLmRep1959()
		{
			var alerts = await PortalClient.GetAlertsAsync(new AlertFilter
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
			}).ConfigureAwait(false);
			CheckAlertsAreValid(alerts);
		}

		// Time needs to be changed
		[Fact]
		public async void GetFilteredAlertsDefaultPeriod()
		{
			var alerts =
				await PortalClient.GetAlertsAsync(new AlertFilter { StartEpochIsAfter = StartDateTimeSeconds, StartEpochIsBefore = EndDateTimeSeconds }).ConfigureAwait(false);
			CheckAlertsAreValid(alerts);
		}

		[Fact]
		public async void GetFilteredAlertsFor2YearsAgoReturnsNoAlerts()
		{
			var alerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartEpochIsAfter = EndDateTime.AddYears(-2).SecondsSinceTheEpoch(),
					StartEpochIsBefore = EndDateTime.AddYears(-2).AddDays(2).SecondsSinceTheEpoch()
				}).ConfigureAwait(false);
			Assert.Empty(alerts);
		}

		[Fact]
		public async void GetFilteredAlertsForAlertsStartedThisWeekButNotCleared()
		{
			var startUtcIsAfterOrAt = EndDateTime.AddDays(-1);
			var alerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartUtcIsAfter = startUtcIsAfterOrAt,
					IsCleared = false,
					IncludeCleared = null
				}).ConfigureAwait(false);
			Assert.True(alerts.All(a => a.StartOnUtc >= startUtcIsAfterOrAt));
			Assert.True(alerts.All(a => !a.IsCleared));
		}

		[Fact]
		public async void GetFilteredAlertsForAlertsStartedThisWeekAndCleared()
		{
			var startUtcIsAfterOrAt = EndDateTime.AddDays(-7);
			var alerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartUtcIsAfter = startUtcIsAfterOrAt,
				IsCleared = true,
				IncludeCleared = null,
			}).ConfigureAwait(false);
			Assert.True(alerts.All(a => a.StartOnUtc >= startUtcIsAfterOrAt));
			Assert.True(alerts.All(a => a.IsCleared));
		}

		[Fact]
		public async void GetFilteredAlertsForDatacenter()
		{
			var alerts =
				await PortalClient.GetAlertsAsync(new AlertFilter
				{
					StartEpochIsAfter = StartDateTimeSeconds,
					StartEpochIsBefore = EndDateTimeSeconds,
					MonitorObjectGroupFullPaths = new List<string> { "Datacenter/*" },
					IncludeCleared = true
				}).ConfigureAwait(false);
			CheckAlertsAreValid(alerts);

			// Refetch with GetAlertAsync
			foreach (var alert in alerts.Take(10))
			{
				var refetchedAlert = await PortalClient.GetAlertAsync(alert.Id).ConfigureAwait(false);
				Assert.NotNull(refetchedAlert);
				Assert.Equal(alert.MonitorObjectId, refetchedAlert.MonitorObjectId);
				Assert.Equal(alert.Id, refetchedAlert.Id);
				Assert.Equal(alert.AlertType, refetchedAlert.AlertType);
				Assert.Equal(alert.DetailMessage?.Body, refetchedAlert.DetailMessage?.Body);
			}
		}

		[Fact]
		public async void GetFilteredAlertsForDev()
		{
			var alerts = await PortalClient.GetAlertsAsync(
				new AlertFilter
				{
					StartEpochIsAfter = 1470009600,
					StartEpochIsBefore = 1471564800,
					MonitorObjectId = WindowsDeviceId,
					Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
				}).ConfigureAwait(false);
			CheckAlertsAreValid(alerts);

			// Refetch with GetAlertAsync
			foreach (var alert in alerts)
			{
				var refetchedAlert = await PortalClient.GetAlertAsync(alert.Id).ConfigureAwait(false);
				Assert.Equal(alert.MonitorObjectId, refetchedAlert.MonitorObjectId);
				Assert.Equal(alert.Id, refetchedAlert.Id);
				Assert.Equal(alert.AlertType, refetchedAlert.AlertType);
				Assert.Equal(alert.DetailMessage?.Body, refetchedAlert.DetailMessage?.Body);
			}
		}

		[Fact]
		public async void GetFilteredAlerts_SpecificLevels()
		{
			var timespan = TimeSpan.FromDays(1);

			var allAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartUtcIsAfter = DateTime.UtcNow - timespan,
				Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error, AlertLevel.Warning }
			}).ConfigureAwait(false);
			CheckAlertsAreValid(allAlerts);

			// Make sure that some are returned with a warning level and some with an error level
			// If not, this test is inconclusive
			if (
				!(allAlerts.Any(a => a.AlertLevel == AlertLevel.Warning)
				  && allAlerts.Any(a => a.AlertLevel == AlertLevel.Error || a.AlertLevel == AlertLevel.Critical)))
			{
				throw new Exception($"Inconclusive: Test portal does not have some warning and some error or above alerts in the last {timespan.Humanize()}.");
			}

			// Get filtered alerts, error and above only
			var errorAndAboveAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartUtcIsAfter = DateTime.UtcNow - timespan,
				Levels = new List<AlertLevel> { AlertLevel.Critical, AlertLevel.Error }
			}).ConfigureAwait(false);

			Logger.LogDebug($"{errorAndAboveAlerts.Count}");

			// Ensure that the number of error and above is fewer than the total
			Assert.True(errorAndAboveAlerts.Count < allAlerts.Count);

			Assert.True(errorAndAboveAlerts.All(a => a.AlertLevel >= AlertLevel.Error));
		}

		[Fact]
		public async void GetAlertsMatchingProblemSignature()
		{
			var dateTimeOffset = DateTimeOffset.UtcNow.AddDays(-7);

			var alerts = await PortalClient.GetAllAsync(new Filter<Alert>
			{
				FilterItems = new List<FilterItem<Alert>>
				{
					new FilterItem<Alert> { Property = nameof(Alert.StartOnSeconds), Operation = ">", Value = dateTimeOffset.ToUnixTimeSeconds()},
					new FilterItem<Alert> { Property = nameof(Alert.InternalId), Operation=":", Value="LMS462482416" },
					new FilterItem<Alert> { Property = nameof(Alert.IsCleared), Operation=":", Value="*" },
				}
			}).ConfigureAwait(false);
			Assert.NotNull(alerts);
		}

		[Fact]
		public void LevelDefaultsToError()
		{
			var alertFilter = new AlertFilter();
			Assert.Equal(2, alertFilter.Levels.Count);
			Assert.Equal(AlertLevel.Error, alertFilter.Levels[0]);
			Assert.Equal(AlertLevel.Critical, alertFilter.Levels[1]);
		}

		[Fact]
		public async void SdtFilter_Works()
		{
			var nowUtc = DateTimeOffset.UtcNow;
			var oneDayAgo = nowUtc.AddDays(-1);

			// Get all alerts
			var allAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
				StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
			}).ConfigureAwait(false);

			// Get all alerts NOT in SDT
			var nonSdtAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
				StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
				SdtFilter = SdtFilter.NonSdt
			}).ConfigureAwait(false);
			Assert.All(nonSdtAlerts, alert => Assert.Null(alert.Sdt));

			// Get all alerts in SDT
			var sdtAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
			{
				StartEpochIsAfter = oneDayAgo.ToUnixTimeSeconds(),
				StartEpochIsBefore = nowUtc.ToUnixTimeSeconds(),
				SdtFilter = SdtFilter.Sdt
			}).ConfigureAwait(false);
			Assert.All(sdtAlerts, alert => Assert.NotNull(alert.Sdt));

			// Make sure the numbers add up
			Assert.Equal(allAlerts.Count, nonSdtAlerts.Count + sdtAlerts.Count);
		}

		[Fact(Skip = "This is only to test specific problems with the /alert/alerts endpoints for R2UT")]
		public async void R2ut_Alerts_Test()
		{
			var startDate = new DateTime(2020, 9, 1, 0, 0, 0);
			var endDate = new DateTime(2020, 10, 1, 0, 0, 0);
			var s = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0, TimeSpan.Zero);
			var e = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, 0, TimeSpan.Zero);

			PortalClient.TimeOut = TimeSpan.FromSeconds(120);
			PortalClient.AttemptCount = 5;

			while (true)
			{
				// Alerts that start inside the range (same as web UI)
				var inRangeAlerts = await PortalClient.GetAlertsAsync(new AlertFilter
				{
					ResourceTemplateName = "Alert History",
					IncludeCleared = true,
					StartUtcIsAfter = s.UtcDateTime,
					StartUtcIsBefore = e.UtcDateTime,
					EndUtcIsAfter = null,
					EndUtcIsBefore = null,
					MonitorObjectGroupFullPaths = new List<string> { DeviceGroupFullPath },
					Levels = new List<AlertLevel> { AlertLevel.Warning, AlertLevel.Error, AlertLevel.Critical }
				})
				.ConfigureAwait(false);

				//Debug.WriteLine("\r\n ******* ALERTS START IN RANGE: " + inRangeAlerts.Count);

				//await Task.Delay(2000).ConfigureAwait(false);
			}
		}
	}
}