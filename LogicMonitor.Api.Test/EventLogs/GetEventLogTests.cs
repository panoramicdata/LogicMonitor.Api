namespace LogicMonitor.Api.Test.EventLogs;
public class GetEventLogTests : TestWithOutput
{
	public GetEventLogTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public void ValidateRegexs()
		=> LogItemExtensions.ValidateRegexs();

	[Fact]
	public async void GetEventLog_Succeeds()
	{

		var endDateTimeUtc = DateTime.UtcNow;
		var startDateTimeUtc = endDateTimeUtc.AddHours(-118);
		var unhandledLogItems = new List<LogItem>();
		for (var i = 0; i < 3000; i += 300)
		{
			var logItems = await LogicMonitorClient
				.GetLogItemsAsync(new LogFilter(i, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc))
				.ConfigureAwait(false);


			foreach (var logItem in logItems)
			{
				try
				{
					var auditEvent = logItem.ToAuditEvent();
					auditEvent.Should().NotBeNull();

					if (auditEvent.MatchedRegExId == -1)
					{
						unhandledLogItems.Add(logItem);
						continue;
					}

					//auditEvent.MatchedRegExId.Should().NotBe(-1);
					auditEvent.OutcomeType.Should().NotBe(AuditEventOutcomeType.None);
					auditEvent.ActionType.Should().NotBe(AuditEventActionType.None);

					if (auditEvent.ActionType != AuditEventActionType.Login
						&& auditEvent.ActionType != AuditEventActionType.GeneralApi)
					{
						auditEvent.EntityType.Should().NotBe(AuditEventEntityType.None);
					}
				}
				catch (Exception ex)
				{

					Logger.LogError(ex, "Could not parse log item {Id}\n'{Description}'", logItem.Id, logItem.Description);
					throw;
				}

			}

		}

		if (unhandledLogItems.Count > 0)
		{
			Logger.LogError("{Count} Unhandled log items", unhandledLogItems.Count);
			foreach (var logItem in unhandledLogItems)
			{
				Logger.LogError("{Id}\n{Description}\n", logItem.Id, logItem.Description);
			}

			Assert.False(true, "Unhandled log items");
		}
	}
}
