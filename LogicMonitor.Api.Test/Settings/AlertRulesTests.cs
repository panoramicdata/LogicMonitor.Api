namespace LogicMonitor.Api.Test.Settings;

public class AlertRulesTests : TestWithOutput
{
	public AlertRulesTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	private static async Task GetAlertRule(LogicMonitorClient portalClient, string alertRuleName, bool enableAlertClear)
	{
		var alertRule = (await portalClient.GetAlertRuleListAsync(new(), default).ConfigureAwait(false)).Items.SingleOrDefault(ar => ar.Name == alertRuleName)
			?? throw new ArgumentException($"No alert rule found with name {alertRuleName}");

		alertRule.SuppressAlertClear = !enableAlertClear;
	}

	[Fact]
	public async Task DisableAndReEnableClearedAlerts()
	{
		var portalClient = LogicMonitorClient;
		await GetAlertRule(portalClient, AlertRuleName, true).ConfigureAwait(false);
		await GetAlertRule(portalClient, AlertRuleName, false).ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAlertRules()
	{
		var alertRules = await LogicMonitorClient.GetAlertRuleListAsync(new(), default).ConfigureAwait(false);
		alertRules.Items.Should().NotBeNullOrEmpty();

		// Get each one individually and check everything matches
		foreach (var alertRule in alertRules.Items)
		{
			// Save it
			await LogicMonitorClient.SaveAlertRuleAsync(alertRule, default).ConfigureAwait(false);

			var refetchedAlertRule = await LogicMonitorClient.GetAlertRuleAsync(alertRule.Id, new Filter<AlertRule>(), default).ConfigureAwait(false);
			refetchedAlertRule.Id.Should().Be(alertRule.Id);
			// Other tests?

			// Only do one for now
			break;
		}
	}

	[Fact]
	public async Task AddAndDeleteAlertRule()
	{
		var newRule = new AlertRule()
		{
			DataPoint = "*",
			DataSourceInstanceName = "*",
			Devices = ["Test Device"],
			DeviceGroups = ["Test Groups"],
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
			.AddAlertRuleAsync(newRule, default)
			.ConfigureAwait(false);

		var alertRules = await LogicMonitorClient
			.GetAlertRuleListAsync(new Filter<AlertRule>(), default)
			.ConfigureAwait (false);

		var found = false;
		var createdAlert = new AlertRule();
		
		foreach (var alertRule in alertRules.Items)
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
				.DeleteAlertRuleAsync(createdAlert.Id, default)
				.ConfigureAwait(false);
		}

		found.Should().BeTrue();

	}
}
