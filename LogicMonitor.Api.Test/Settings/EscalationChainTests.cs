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
			.GetAllAsync<EscalationChain>(CancellationToken.None)
			.ConfigureAwait(false);
		escalationChains.Should().NotBeNullOrEmpty();
	}
}
