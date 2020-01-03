using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class LogicModuleUpdateTests : TestWithOutput
	{
		public LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		// See the LogicModuleUpdateTests.cs in LogicMonitor.Api.Test.LogicModules
		// This is now a POST and returns a relevant details

		//[Fact]
		//public async void GetAll()
		//{
		//	var logicModuleUpdates = await PortalClient.GetAllAsync(new Filter<LogicModuleUpdate>
		//	{
		//		Order = new Order<LogicModuleUpdate>
		//		{
		//			Property = nameof(LogicModuleUpdate.Name),
		//			Direction = OrderDirection.Asc
		//		}
		//	}).ConfigureAwait(false);

		//	// Make sure that some are returned
		//	Assert.True(logicModuleUpdates.Count > 0);

		//	// Make sure that all have Unique locators
		//	Assert.False(logicModuleUpdates.Select(a => a.Locator).HasDuplicates());
		//}
	}
}