namespace LogicMonitor.Api.Test.Settings;

public class AlertRulesTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private static async Task GetAlertRule(LogicMonitorClient portalClient, string alertRuleName, bool enableAlertClear)
	{
		var alertRules = await portalClient
				.GetAllAsync<AlertRule>(default)
				.ConfigureAwait(true);

		var alertRule = alertRules.SingleOrDefault(ar => ar.Name == alertRuleName);
		alertRule.Should().NotBeNull();
		alertRule.SuppressAlertClear = !enableAlertClear;
	}

	[Fact]
	public async Task DisableAndReEnableClearedAlerts()
	{
		var portalClient = LogicMonitorClient;
		await GetAlertRule(portalClient, AlertRuleName, true)
			.ConfigureAwait(true);
		await GetAlertRule(portalClient, AlertRuleName, false)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetAlertRules()
	{
		var alertRules = await LogicMonitorClient
			.GetAllAsync<AlertRule>(default)
			.ConfigureAwait(true);
		alertRules.Should().NotBeNullOrEmpty();

		// Get each one individually and check everything matches
		foreach (var alertRule in alertRules)
		{
			// Save it
			await LogicMonitorClient
				.SaveAlertRuleAsync(alertRule, default)
				.ConfigureAwait(true);

			var refetchedAlertRule = await LogicMonitorClient
				.GetAsync<AlertRule>(alertRule.Id, default)
				.ConfigureAwait(true);
			refetchedAlertRule.Id.Should().Be(alertRule.Id);
			// Other tests?

			// Only do one for now
			break;
		}
	}

	[Fact]
	public async Task AddAndDeleteAlertRule()
	{
		var newRule = new AlertRuleCreationDto()
		{
			DataPoint = "*",
			DataSourceInstanceName = "*",
			Resources = ["Test Device"],
			ResourceGroups = ["Test Groups"],
			EscalationChainId = 67,
			SendAnomalySuppressedAlert = false,
			Priority = 100,
			SuppressAlertAckSdt = false,
			DataSourceName = "*",
			SuppressAlertClear = false,
			Name = "LornaTest",
			LevelString = "All",
		};

		await LogicMonitorClient
			.CreateAsync(newRule, default)
			.ConfigureAwait(true);

		var alertRules = await LogicMonitorClient
			.GetAllAsync<AlertRule>(default)
			.ConfigureAwait(true);
		var found = false;
		var createdAlert = new AlertRule();

		foreach (var alertRule in alertRules)
		{
			if (alertRule.Name.Equals("LornaTest", StringComparison.Ordinal))
			{
				found = true;
				createdAlert = alertRule;
				break;
			}
		}

		if (found)
		{
			await LogicMonitorClient
				.DeleteAsync<AlertRule>(createdAlert.Id, default)
				.ConfigureAwait(true);
		}

		found.Should().BeTrue();

	}
}
