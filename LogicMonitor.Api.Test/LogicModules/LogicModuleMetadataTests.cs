using LogicMonitor.Api.LogicModules;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
	public class LogicModuleMetadataTests : TestWithOutput
	{
		public LogicModuleMetadataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		//[Fact]
		//public async void GetDataSourceMetadata()
		//{
		//	var dataSource = await DefaultPortalClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-").ConfigureAwait(false);
		//	Assert.NotNull(dataSource);
		//	Assert.NotEqual(0, dataSource.Id);
		//	var logicModuleMetadata = await DefaultPortalClient.GetLogicModuleMetadata(LogicModuleType.DataSource, dataSource.Id).ConfigureAwait(false);
		//	CheckMetadata(logicModuleMetadata);
		//}

		//[Fact(Skip = "LogicMonitor is broken - ZenDesk ticket number 112910")]
		//public async void GetEventSourceMetadata()
		//{
		//	var eventSource = await DefaultPortalClient.GetByNameAsync<EventSource>("Windows System Event Log").ConfigureAwait(false);
		//	Assert.NotNull(eventSource);
		//	Assert.NotEqual(0, eventSource.Id);
		//	var logicModuleMetadata = await DefaultPortalClient.GetLogicModuleMetadata(LogicModuleType.EventSource, eventSource.Id).ConfigureAwait(false);
		//	CheckMetadata(logicModuleMetadata);
		//}

		//[Fact]
		//public async void GetConfigSourceMetadata()
		//{
		//	var configSource = await PortalClient.GetByNameAsync<ConfigSource>("Cisco_IOS").ConfigureAwait(false);
		//	Assert.NotNull(configSource);
		//	Assert.NotEqual(0, configSource.Id);
		//	var logicModuleMetadata = await PortalClient.GetLogicModuleMetadata(LogicModuleType.ConfigSource, configSource.Id).ConfigureAwait(false);
		//	CheckMetadata(logicModuleMetadata);
		//}

		[Fact]
		public async void GetPropertySourceMetadata()
		{
			var propertySource = await PortalClient.GetByNameAsync<PropertySource>("Cisco_Product_Info").ConfigureAwait(false);
			Assert.NotNull(propertySource);
			Assert.NotEqual(0, propertySource.Id);
			var logicModuleMetadata = await PortalClient.GetLogicModuleMetadata(LogicModuleType.PropertySource, propertySource.Id).ConfigureAwait(false);
			CheckMetadata(logicModuleMetadata);
		}

		private static void CheckMetadata(LogicModuleMetadata logicModuleMetadata)
		{
			Assert.NotNull(logicModuleMetadata);
			Assert.NotNull(logicModuleMetadata.LmLocator);
			Assert.NotNull(logicModuleMetadata.Namespace);
			Assert.NotNull(logicModuleMetadata.Quality);
			Assert.NotNull(logicModuleMetadata.RegistryVersion);
		}
	}
}