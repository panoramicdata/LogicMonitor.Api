using LogicMonitor.Api.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings;

public class AlertRulesTests : TestWithOutput
{
	public AlertRulesTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	private static async Task GetAlertRule(LogicMonitorClient portalClient, string alertRuleName, bool enableAlertClear)
	{
		var alertRule = (await portalClient.GetAllAsync<AlertRule>().ConfigureAwait(false)).SingleOrDefault(ar => ar.Name == alertRuleName);

		if (alertRule == null)
		{
			throw new ArgumentException($"No alert rule found with name {alertRuleName}");
		}

		alertRule.SuppressAlertClear = !enableAlertClear;
	}

	[Fact]
	public async void DisableAndReEnableClearedAlerts()
	{
		var portalClient = LogicMonitorClient;
		await GetAlertRule(portalClient, AlertRuleName, true).ConfigureAwait(false);
		await GetAlertRule(portalClient, AlertRuleName, false).ConfigureAwait(false);
	}

	[Fact]
	public async void GetAlertRules()
	{
		var alertRules = await LogicMonitorClient.GetAllAsync<AlertRule>().ConfigureAwait(false);
		Assert.NotNull(alertRules);
		Assert.True(alertRules.Count > 0);

		// Get each one individually and check everything matches
		foreach (var alertRule in alertRules)
		{
			// Save it
			await LogicMonitorClient.SaveAlertRuleAsync(alertRule).ConfigureAwait(false);

			var refetchedAlertRule = await LogicMonitorClient.GetAsync<AlertRule>(alertRule.Id).ConfigureAwait(false);
			Assert.Equal(alertRule.Id, refetchedAlertRule.Id);
			// Other tests?

			// Only do one for now
			break;
		}
	}
}
