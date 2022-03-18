namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleUpdateTests : TestWithOutput
{
	public LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	/// <summary>
	/// Get DataSource updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleDataSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var dataSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.DataSource, version.Version.Major, default)
				.ConfigureAwait(false);

		dataSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get EventSource updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleEventSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var eventSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.EventSource, version.Version.Major, default)
				.ConfigureAwait(false);

		eventSourceUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get ConfigSource updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleConfigSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var configSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.ConfigSource, version.Version.Major, default)
				.ConfigureAwait(false);

		configSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get PropertySource updates
	/// </summary>
	[Fact]
	public async void GetLogicModulePropertySourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var propertySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(false);

		propertySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get TopologySource updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleTopologySourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var topologySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.TopologySource, version.Version.Major, default)
				.ConfigureAwait(false);

		topologySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get Job Monitor updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleJobMonitorUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var _ =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(false);

		//jobMonitorUpdates.Items.Should().NotBeNullOrEmpty();	// Usually none
	}

	/// <summary>
	/// Get AppliesTo Function updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleAppliesToUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var _ =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.AppliesToFunction, version.Version.Major, default)
				.ConfigureAwait(false);

		//appliesToUpdates.Items.Should().NotBeNullOrEmpty();	// Usually none
	}

	/// <summary>
	/// Get SnmpSysOID updates
	/// </summary>
	[Fact]
	public async void GetLogicModuleSnmpSysOidUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var snmpSysOidUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.SnmpSysOIDMap, version.Version.Major, default)
				.ConfigureAwait(false);

		snmpSysOidUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get ALL LogicModule updates
	/// </summary>
	[Fact]
	public async void GetAllLogicModuleUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var allUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.All, version.Version.Major, default)
				.ConfigureAwait(false);

		allUpdates.Total.Should().BePositive();
	}

	/// <summary>
	/// Find one unaudited data source update and mark as audited
	/// </summary>
	[Fact]
	public async void AuditDataSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.DataSource, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedNotInUse)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			var dataSourceToAudit = dataSourceUpdates[0];
			var auditedDataSource =
				await LogicMonitorClient.AuditDataSource(
					dataSourceToAudit.LocalId,
					dataSourceToAudit.Version,
					default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one unaudited event source update and mark as audited
	/// </summary>
	[Fact]
	public async void AuditEventSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.EventSource, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			var eventSourceToAudit = eventSourceUpdates[0];
			var auditedEventSource =
				await LogicMonitorClient.AuditEventSource(
					eventSourceToAudit.LocalId,
					eventSourceToAudit.Version,
					default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one unaudited config source update and mark as audited
	/// </summary>
	[Fact]
	public async void AuditConfigSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.ConfigSource, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			var configSourceToAudit = configSourceUpdates[0];
			var auditedConfigSource =
				await LogicMonitorClient.AuditConfigSource(
					configSourceToAudit.LocalId,
					configSourceToAudit.Version,
					default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one unaudited property source update and mark as audited
	/// </summary>
	[Fact]
	public async void AuditPropertySource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			var propertySourceToAudit = propertySourceUpdates[0];
			var auditedPropertySource =
				await LogicMonitorClient.AuditPropertySource(
					propertySourceToAudit.LocalId,
					propertySourceToAudit.Version,
					default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Data Source and import
	/// </summary>
	[Fact]
	public async void ImportDataSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.DataSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						dataSourceUpdates[0].Name
				},
				version.Version.Major,
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Event Source and import
	/// </summary>
	[Fact]
	public async void ImportEventSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.EventSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						eventSourceUpdates[0].Name
				},
				version.Version.Major,
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Config Source and import
	/// </summary>
	[Fact]
	public async void ImportConfigSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.ConfigSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						configSourceUpdates[0].Name
				},
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Property Source and import
	/// </summary>
	[Fact]
	public async void ImportPropertySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.PropertySource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						propertySourceUpdates[0].Name
				},
				version.Version.Major,
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Topology Source and import
	/// </summary>
	[Fact]
	public async void ImportTopologySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.TopologySource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var topologySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (topologySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						topologySourceUpdates[0].Name
				},
				version.Version.Major,
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated Job Monitor and import
	/// </summary>
	[Fact]
	public async void ImportJobMonitor()
	{
		const LogicModuleType logicModuleType = LogicModuleType.JobMonitor;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var jobMonitorUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (jobMonitorUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						jobMonitorUpdates[0].Name
				},
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated AppliesToFunction and import
	/// </summary>
	[Fact]
	public async void ImportAppliesToFunction()
	{
		const LogicModuleType logicModuleType = LogicModuleType.AppliesToFunction;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var appliesToFunctionUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (appliesToFunctionUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModules(
				logicModuleType,
				new List<string>
				{
						appliesToFunctionUpdates[0].Name
				},
				default)
				.ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Find one updated SNMP SysOID Map and import
	/// </summary>
	[Fact]
	public async void ImportSnmpSysOidMap()
	{
		const LogicModuleType logicModuleType = LogicModuleType.SnmpSysOIDMap;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(false);

		var snmpSysOidMapUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdates(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(false))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (snmpSysOidMapUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportSnmpSysOidMap(
				new List<SnmpSysOidMapImportItem>
				{
						new SnmpSysOidMapImportItem
						{
							Id = snmpSysOidMapUpdates[0].LocalId,
							Oid = snmpSysOidMapUpdates[0].Name
						}
				},
				version.Version.Major,
				default)
				.ConfigureAwait(false);
		}
	}
}
