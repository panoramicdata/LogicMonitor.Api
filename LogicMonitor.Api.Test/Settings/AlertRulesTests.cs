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

			var refetchedAlertRule = await LogicMonitorClient.GetAlertRuleAsync(alertRule.Id, default).ConfigureAwait(false);
			refetchedAlertRule.Id.Should().Be(alertRule.Id);
			// Other tests?

			// Only do one for now
			break;
		}
	}
}
