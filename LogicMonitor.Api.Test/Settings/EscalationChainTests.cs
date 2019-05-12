using LogicMonitor.Api.Alerts;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class EscalationChainTests : TestWithOutput
	{
		public EscalationChainTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAll()
		{
			var escalationChains = await PortalClient.GetAllAsync<EscalationChain>().ConfigureAwait(false);
			Assert.NotNull(escalationChains);
			Assert.True(escalationChains.Count > 0);
		}
	}
}