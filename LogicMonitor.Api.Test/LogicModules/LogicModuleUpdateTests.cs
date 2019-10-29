using LogicMonitor.Api.LogicModules;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
	public class LogicModuleUpdateTests : TestWithOutput
	{
		public LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetLogicModuleUpdates()
		{
			var items = await PortalClient.GetAllAsync<LogicModuleUpdate>().ConfigureAwait(false);

			Assert.NotEmpty(items);
			Assert.True(items.Count > 1);
		}
	}
}