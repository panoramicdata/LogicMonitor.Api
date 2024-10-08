namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleMetadataTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDataSourceMetadata()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		dataSource.Id.Should().NotBe(0);
		var logicModuleMetadata = await LogicMonitorClient
			.GetLogicModuleMetadataAsync(LogicModuleType.DataSource, dataSource.Id, default)
			.ConfigureAwait(true);
		CheckMetadata(logicModuleMetadata);
	}

	//[Fact(Skip = "LogicMonitor is broken - ZenDesk ticket number 112910")]
	//public async Task GetEventSourceMetadata()
	//{
	//	var eventSource = await DefaultPortalClient.GetByNameAsync<EventSource>("Windows System Event Log").ConfigureAwait(true);
	//	eventSource.Should().NotBeNull();
	//	eventSource.Id.Should().NotBe(0);
	//	var logicModuleMetadata = await DefaultPortalClient.GetLogicModuleMetadata(LogicModuleType.EventSource, eventSource.Id).ConfigureAwait(true);
	//	CheckMetadata(logicModuleMetadata);
	//}

	[Fact]
	public async Task GetConfigSourceMetadata()
	{
		var configSource = await LogicMonitorClient
			.GetByNameAsync<ConfigSource>("Cisco_IOS", default)
			.ConfigureAwait(true);
		configSource.Should().NotBeNull();
		if (configSource != null)
		{
			configSource.Id.Should().NotBe(0);
			var logicModuleMetadata = await LogicMonitorClient
				.GetLogicModuleMetadataAsync(LogicModuleType.ConfigSource, configSource.Id, default)
				.ConfigureAwait(true);
			CheckMetadata(logicModuleMetadata);
		}
	}

	[Fact]
	public async Task GetPropertySourceMetadata()
	{
		var propertySource = await LogicMonitorClient
			.GetByNameAsync<PropertySource>("Cisco_Product_Info", default)
			.ConfigureAwait(true);
		propertySource.Should().NotBeNull();
		if (propertySource != null)
		{
			propertySource.Id.Should().NotBe(0);
			var logicModuleMetadata = await LogicMonitorClient
				.GetLogicModuleMetadataAsync(LogicModuleType.PropertySource, propertySource.Id, default)
				.ConfigureAwait(true);
			CheckMetadata(logicModuleMetadata);
		}
	}

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
