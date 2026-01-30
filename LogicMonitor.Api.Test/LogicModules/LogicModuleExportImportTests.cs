namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleExportImportTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetDataSourceJson()
	{
		// Get a DataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();

		// Export as JSON
		var json = await LogicMonitorClient
			.GetDataSourceJsonAsync(dataSource!.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();
		json.Should().Contain("Ping");

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
		jObject["name"]?.ToString().Should().Contain("Ping");
	}

	[Fact]
	public async Task GetEventSourceJson()
	{
		// Get an EventSource
		var eventSources = await LogicMonitorClient
			.GetAllAsync<EventSource>(CancellationToken);

		if (eventSources.Count == 0)
		{
			// Skip if no EventSources exist
			return;
		}

		var eventSource = eventSources[0];

		// Export as JSON
		var json = await LogicMonitorClient
			.GetEventSourceJsonAsync(eventSource.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task GetConfigSourceJson()
	{
		// Get a ConfigSource
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(CancellationToken);

		if (configSources.Count == 0)
		{
			// Skip if no ConfigSources exist
			return;
		}

		var configSource = configSources[0];

		// Export as JSON
		var json = await LogicMonitorClient
			.GetConfigSourceJsonAsync(configSource.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task GetPropertySourceJson()
	{
		// Get a PropertySource
		var propertySources = await LogicMonitorClient
			.GetAllAsync<PropertySource>(CancellationToken);

		if (propertySources.Count == 0)
		{
			// Skip if no PropertySources exist
			return;
		}

		var propertySource = propertySources[0];

		// Export as JSON (PropertySources already return JSON)
		var json = await LogicMonitorClient
			.GetPropertySourceJsonAsync(propertySource.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task GetLogicModuleJsonByType()
	{
		// Get a DataSource using the generic method
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();

		// Export using the type-based method
		var json = await LogicMonitorClient
			.GetLogicModuleJsonAsync(LogicModuleType.DataSource, dataSource!.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task CompareJsonAndXmlExport()
	{
		// Get a DataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();

		// Export as JSON
		var json = await LogicMonitorClient
			.GetDataSourceJsonAsync(dataSource!.Id, CancellationToken);

		// Export as XML
		var xml = await LogicMonitorClient
			.GetDataSourceXmlAsync(dataSource.Id, CancellationToken);

		// Both should be non-empty
		json.Should().NotBeNullOrEmpty();
		xml.Should().NotBeNullOrEmpty();

		// JSON should start with { and XML should start with <
		json.Trim().Should().StartWith("{");
		xml.Trim().Should().StartWith("<");

		// Log for inspection
		Logger.LogInformation("JSON export length: {Length}", json.Length);
		Logger.LogInformation("XML export length: {Length}", xml.Length);
	}

	[Fact]
	public async Task GetAppliesToFunctionJson()
	{
		// Get an AppliesToFunction
		var functions = await LogicMonitorClient
			.GetAllAsync<AppliesToFunction>(CancellationToken);

		if (functions.Count == 0)
		{
			// Skip if no functions exist
			return;
		}

		var function = functions[0];

		// Export as JSON
		var json = await LogicMonitorClient
			.GetAppliesToFunctionJsonAsync(function.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTopologySourceJson()
	{
		// Get a TopologySource
		var topologySources = await LogicMonitorClient
			.GetAllAsync<TopologySource>(CancellationToken);

		if (topologySources.Count == 0)
		{
			// Skip if no TopologySources exist
			return;
		}

		var topologySource = topologySources[0];

		// Export as JSON
		var json = await LogicMonitorClient
			.GetTopologySourceJsonAsync(topologySource.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task GetJobMonitorJson()
	{
		// Get a JobMonitor
		var jobMonitors = await LogicMonitorClient
			.GetAllAsync<JobMonitor>(CancellationToken);

		if (jobMonitors.Count == 0)
		{
			// Skip if no JobMonitors exist
			return;
		}

		var jobMonitor = jobMonitors[0];

		// Export as JSON
		var json = await LogicMonitorClient
			.GetJobMonitorJsonAsync(jobMonitor.Id, CancellationToken);

		json.Should().NotBeNullOrEmpty();

		// Verify it's valid JSON
		var jObject = JObject.Parse(json);
		jObject.Should().NotBeNull();
	}

	[Fact]
	public async Task ExportDataSourceToJsonFile()
	{
		// Get a DataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();

		// Create a temp file path
		var tempFilePath = Path.Combine(Path.GetTempPath(), $"datasource_{dataSource!.Id}.json");

		try
		{
			// Export to file
			await LogicMonitorClient
				.ExportLogicModuleToJsonFileAsync(
					LogicModuleType.DataSource,
					dataSource.Id,
					tempFilePath,
					CancellationToken);

			// Verify file was created
			File.Exists(tempFilePath).Should().BeTrue();

			// Read and verify content
			var fileContent = File.ReadAllText(tempFilePath);
			fileContent.Should().NotBeNullOrEmpty();

			// Verify it's valid JSON
			var jObject = JObject.Parse(fileContent);
			jObject.Should().NotBeNull();

			Logger.LogInformation("Exported DataSource to: {FilePath}", tempFilePath);
			Logger.LogInformation("File size: {Size} bytes", new FileInfo(tempFilePath).Length);
		}
		finally
		{
			// Cleanup
			if (File.Exists(tempFilePath))
			{
				File.Delete(tempFilePath);
			}
		}
	}

	[Fact]
	public async Task ExportDataSourceToXmlFile()
	{
		// Get a DataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();

		// Create a temp file path
		var tempFilePath = Path.Combine(Path.GetTempPath(), $"datasource_{dataSource!.Id}.xml");

		try
		{
			// Export to file
			await LogicMonitorClient
				.ExportLogicModuleToXmlFileAsync(
					LogicModuleType.DataSource,
					dataSource.Id,
					tempFilePath,
					CancellationToken);

			// Verify file was created
			File.Exists(tempFilePath).Should().BeTrue();

			// Read and verify content
			var fileContent = File.ReadAllText(tempFilePath);
			fileContent.Should().NotBeNullOrEmpty();
			fileContent.Trim().Should().StartWith("<");

			Logger.LogInformation("Exported DataSource to: {FilePath}", tempFilePath);
			Logger.LogInformation("File size: {Size} bytes", new FileInfo(tempFilePath).Length);
		}
		finally
		{
			// Cleanup
			if (File.Exists(tempFilePath))
			{
				File.Delete(tempFilePath);
			}
		}
	}

	// Note: Import tests are skipped by default to avoid modifying the portal
	// Uncomment and run manually when needed

	//[Fact(Skip = "Don't import without understanding the implications")]
	//public async Task ImportDataSourceFromJson()
	//{
	//	// First export a DataSource
	//	var dataSource = await LogicMonitorClient
	//		.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
	//	dataSource.Should().NotBeNull();
	//
	//	var json = await LogicMonitorClient
	//		.GetDataSourceJsonAsync(dataSource!.Id, CancellationToken);
	//
	//	// Modify the name to avoid conflicts
	//	var jObject = JObject.Parse(json);
	//	jObject["name"] = "Ping_Test_Copy_" + Guid.NewGuid().ToString("N").Substring(0, 8);
	//	jObject["displayedAs"] = "Ping Test Copy";
	//	var modifiedJson = jObject.ToString();
	//
	//	// Import (this will create a new DataSource)
	//	var imported = await LogicMonitorClient
	//		.ImportDataSourceJsonAsync(modifiedJson, CancellationToken);
	//
	//	imported.Should().NotBeNull();
	//	imported.Id.Should().BePositive();
	//
	//	// Cleanup: delete the imported DataSource
	//	await LogicMonitorClient.DeleteAsync<DataSource>(imported.Id, CancellationToken);
	//}
}
