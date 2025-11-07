namespace LogicMonitor.Api.Test.Settings;

public class EscalationChainTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAll()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), CancellationToken);
		escalationChains.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetEscalationChain()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), CancellationToken);

		var chain = await LogicMonitorClient
			.GetEscalationChainAsync(escalationChains.Items[0].Id, CancellationToken);

		chain.Should().NotBeNull();
	}
}
