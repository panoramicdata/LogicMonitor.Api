namespace LogicMonitor.Api.Test.Settings;

public class EscalationChainTests : TestWithOutput
{
	public EscalationChainTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var escalationChains = await LogicMonitorClient.GetAllAsync<EscalationChain>().ConfigureAwait(false);
		Assert.NotNull(escalationChains);
		Assert.True(escalationChains.Count > 0);
	}
}
