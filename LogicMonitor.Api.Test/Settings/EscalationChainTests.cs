namespace LogicMonitor.Api.Test.Settings;

public class EscalationChainTests : TestWithOutput
{
	public EscalationChainTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), default)
			.ConfigureAwait(false);
		escalationChains.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetEscalationChain()
	{
		var escalationChains = await LogicMonitorClient
			.GetEscalationChainsPageAsync(new Filter<EscalationChain>(), default)
			.ConfigureAwait(false);

		var chain = await LogicMonitorClient
			.GetEscalationChainAsync(escalationChains.Items[0].Id, default)
			.ConfigureAwait(false);

		chain.Should().NotBeNull();
	}
}
