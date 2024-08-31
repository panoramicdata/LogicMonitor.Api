namespace LogicMonitor.Api.Test.Settings;

public class EscalationChainTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAll()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), default)
			.ConfigureAwait(true);
		escalationChains.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetEscalationChain()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), default)
			.ConfigureAwait(true);

		var chain = await LogicMonitorClient
			.GetEscalationChainAsync(escalationChains.Items[0].Id, default)
			.ConfigureAwait(true);

		chain.Should().NotBeNull();
	}
}
