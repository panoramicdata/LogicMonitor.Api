namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{

	/// <summary>
	/// Get DataSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleDataSourceUpdates()
	{
		var dataSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.DataSource, CancellationToken);

		dataSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get EventSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleEventSourceUpdates()
	{
		var eventSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.EventSource, CancellationToken);

		eventSourceUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get ConfigSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleConfigSourceUpdates()
	{
		var configSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.ConfigSource, CancellationToken);

		configSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get PropertySource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModulePropertySourceUpdates()
	{
		var propertySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, CancellationToken);

		propertySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get TopologySource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleTopologySourceUpdates()
	{
		var topologySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.TopologySource, CancellationToken);

		topologySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get Job Monitor updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleJobMonitorUpdates()
	{
		var propertySourceUpdates = await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, CancellationToken);

		propertySourceUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get AppliesTo Function updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleAppliesToUpdates()
	{
		var appliesToUpdates = await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.AppliesToFunction, CancellationToken);

		appliesToUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get SnmpSysOID updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleSnmpSysOidUpdates()
	{
		var snmpSysOidUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.SnmpSysOIDMap, CancellationToken);

		snmpSysOidUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get ALL LogicModule updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetAllLogicModuleUpdates()
	{
		var allUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.All, CancellationToken);

		allUpdates.Should().NotBeNull();
	}

	/// <summary>
	/// Find one unaudited data source update and mark as audited
	/// </summary>
	[Fact(Skip = "Don't audit without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task AuditDataSource()
	{
		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.DataSource, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedNotInUse)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			var dataSourceToAudit = dataSourceUpdates[0];
			_ = await LogicMonitorClient.AuditDataSourceAsync(
					dataSourceToAudit.LocalId,
					dataSourceToAudit.Version,
					default)
				;
		}
	}

	/// <summary>
	/// Find one unaudited event source update and mark as audited
	/// </summary>
	[Fact(Skip = "Don't audit without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task AuditEventSource()
	{
		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.EventSource, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			var eventSourceToAudit = eventSourceUpdates[0];
			_ = await LogicMonitorClient.AuditEventSourceAsync(
					eventSourceToAudit.LocalId,
					eventSourceToAudit.Version,
					default)
				;
		}
	}

	/// <summary>
	/// Find one unaudited config source update and mark as audited
	/// </summary>
	[Fact(Skip = "Don't audit without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task AuditConfigSource()
	{
		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.ConfigSource, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			var configSourceToAudit = configSourceUpdates[0];
			_ =
				await LogicMonitorClient.AuditConfigSourceAsync(
					configSourceToAudit.LocalId,
					configSourceToAudit.Version,
					default)
				;
		}
	}

	/// <summary>
	/// Find one unaudited property source update and mark as audited
	/// </summary>
	[Fact(Skip = "Don't audit without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task AuditPropertySource()
	{
		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			var propertySourceToAudit = propertySourceUpdates[0];
			_ = await LogicMonitorClient.AuditPropertySourceAsync(
					propertySourceToAudit.LocalId,
					propertySourceToAudit.Version,
					default)
				;
		}
	}

	/// <summary>
	/// Find one updated Data Source and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportDataSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.DataSource;

		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
					dataSourceUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated Event Source and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportEventSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.EventSource;

		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						eventSourceUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated Config Source and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportConfigSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.ConfigSource;

		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						configSourceUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated Property Source and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportPropertySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.PropertySource;

		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						propertySourceUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated Topology Source and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportTopologySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.TopologySource;

		var topologySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (topologySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						topologySourceUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated Job Monitor and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportJobMonitor()
	{
		const LogicModuleType logicModuleType = LogicModuleType.JobMonitor;

		var jobMonitorUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (jobMonitorUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
					jobMonitorUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated AppliesToFunction and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportAppliesToFunction()
	{
		const LogicModuleType logicModuleType = LogicModuleType.AppliesToFunction;

		var appliesToFunctionUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (appliesToFunctionUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
					appliesToFunctionUpdates[0].Name
				],
				default)
				;
		}
	}

	/// <summary>
	/// Find one updated SNMP SysOID Map and import
	/// </summary>
	[Fact(Skip = "Don't update without understanding the implications")]
	[Trait("Long Tests", "")]
	public async Task ImportSnmpSysOidMap()
	{
		const LogicModuleType logicModuleType = LogicModuleType.SnmpSysOIDMap;

		var snmpSysOidMapUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, CancellationToken)
				)
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (snmpSysOidMapUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportSnmpSysOidMapAsync(
				[
					new SnmpSysOidMapImportItem
					{
						Id = snmpSysOidMapUpdates[0].LocalId,
						Oid = snmpSysOidMapUpdates[0].Name
					}
				],
				default)
				;
		}
	}
}