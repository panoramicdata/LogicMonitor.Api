using LogicMonitor.Api.LogicModules;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
	public class TopologySourceTests : TestWithOutput
	{
		public TopologySourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void BasicTopologySourceTest()
		{
			var _ = await LogicMonitorClient.GetAllAsync<TopologySource>().ConfigureAwait(false);
		}
	}
}