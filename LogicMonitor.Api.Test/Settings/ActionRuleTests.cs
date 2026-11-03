namespace LogicMonitor.Api.Test.Settings;

public class ActionRuleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllActionChains_Succeeds()
	{
		var actionChains = await LogicMonitorClient.GetAllAsync<ActionChain>(CancellationToken);

		actionChains.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllActionRules_Succeeds()
	{
		var actionRules = await LogicMonitorClient.GetAllAsync<ActionRule>(CancellationToken);

		actionRules.Should().NotBeNull();
		foreach (var actionRule in actionRules)
		{
			var status = await LogicMonitorClient.GetActionRuleStatusAsync(actionRule.Id, CancellationToken);
			status.Enabled.Should().Be(actionRule.Enabled);
		}
	}
}
