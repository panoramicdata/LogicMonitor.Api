namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleMetadataTests : TestWithOutput
{
	public LogicModuleMetadataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDataSourceMetadata()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken.None)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		dataSource.Id.Should().NotBe(0);
		var logicModuleMetadata = await LogicMonitorClient
			.GetLogicModuleMetadata(LogicModuleType.DataSource, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);
		CheckMetadata(logicModuleMetadata);
	}

	//[Fact(Skip = "LogicMonitor is broken - ZenDesk ticket number 112910")]
	//public async Task GetEventSourceMetadata()
	//{
	//	var eventSource = await DefaultPortalClient.GetByNameAsync<EventSource>("Windows System Event Log").ConfigureAwait(false);
	//	eventSource.Should().NotBeNull();
	//	eventSource.Id.Should().NotBe(0);
	//	var logicModuleMetadata = await DefaultPortalClient.GetLogicModuleMetadata(LogicModuleType.EventSource, eventSource.Id).ConfigureAwait(false);
	//	CheckMetadata(logicModuleMetadata);
	//}

	//[Fact]
	//public async Task GetConfigSourceMetadata()
	//{
	//	var configSource = await PortalClient.GetByNameAsync<ConfigSource>("Cisco_IOS").ConfigureAwait(false);
	//	configSource.Should().NotBeNull();
	//	configSource.Id.Should().NotBe(0);
	//	var logicModuleMetadata = await PortalClient.GetLogicModuleMetadata(LogicModuleType.ConfigSource, configSource.Id).ConfigureAwait(false);
	//	CheckMetadata(logicModuleMetadata);
	//}

	//[Fact]
	//public async Task GetPropertySourceMetadata()
	//{
	//	var propertySource = await PortalClient.GetByNameAsync<PropertySource>("Cisco_Product_Info").ConfigureAwait(false);
	//	propertySource.Should().NotBeNull();
	//	propertySource.Id.Should().NotBe(0);
	//	var logicModuleMetadata = await PortalClient.GetLogicModuleMetadata(LogicModuleType.PropertySource, propertySource.Id).ConfigureAwait(false);
	//	CheckMetadata(logicModuleMetadata);
	//}

	private static void CheckMetadata(LogicModuleMetadata logicModuleMetadata)
	{
		logicModuleMetadata.Should().NotBeNull();
		logicModuleMetadata.LmLocator.Should().NotBeNull();
		logicModuleMetadata.Namespace.Should().NotBeNull();
		logicModuleMetadata.Quality.Should().NotBeNull();
		logicModuleMetadata.RegistryVersion.Should().NotBeNull();
		logicModuleMetadata.Id.Should().NotBeNull();
	}
}
