using LogicMonitor.Api.LogicModules;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
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
			var dataSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.DataSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(dataSourceUpdates.Items);
		}

		/// <summary>
		/// Get EventSource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleEventSourceUpdates()
		{
			var eventSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.EventSource, default)
					.ConfigureAwait(false);

			Assert.NotNull(eventSourceUpdates.Items);
		}

		/// <summary>
		/// Get ConfigSource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleConfigSourceUpdates()
		{
			var configSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.ConfigSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(configSourceUpdates.Items);
		}

		/// <summary>
		/// Get PropertySource updates
		/// </summary>
		[Fact]
		public async void GetLogicModulePropertySourceUpdates()
		{
			var propertySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(propertySourceUpdates.Items);
		}

		/// <summary>
		/// Get TopologySource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleTopologySourceUpdates()
		{
			var topologySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.TopologySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(topologySourceUpdates.Items);
		}

		/// <summary>
		/// Get Job Monitor updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleJobMonitorUpdates()
		{
			var _ =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(jobMonitorUpdates.Items);	// Usually none
		}

		/// <summary>
		/// Get AppliesTo Function updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleAppliesToUpdates()
		{
			var _ =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.AppliesToFunction, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(appliesToUpdates.Items);	// Usually none
		}

		/// <summary>
		/// Get SnmpSysOID updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleSnmpSysOidUpdates()
		{
			var snmpSysOidUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.SnmpSysOIDMap, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(snmpSysOidUpdates.Items);
		}

		/// <summary>
		/// Get ALL LogicModule updates
		/// </summary>
		[Fact]
		public async void GetAllLogicModuleUpdates()
		{
			var allUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.All, default)
					.ConfigureAwait(false);

			Assert.True(allUpdates.Total > 0);
		}

		/// <summary>
		/// Find one unaudited data source update and mark as audited
		/// </summary>
		[Fact]
		public async void AuditDataSource()
		{
			var dataSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.DataSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedNotInUse)
				.ToList();

			if (dataSourceUpdates.Count > 0)
			{
				var dataSourceToAudit = dataSourceUpdates[0];
				var auditedDataSource =
					await PortalClient.AuditDataSource(
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
			var eventSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.EventSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (eventSourceUpdates.Count > 0)
			{
				var eventSourceToAudit = eventSourceUpdates[0];
				var auditedEventSource =
					await PortalClient.AuditEventSource(
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
			var configSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.ConfigSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (configSourceUpdates.Count > 0)
			{
				var configSourceToAudit = configSourceUpdates[0];
				var auditedConfigSource =
					await PortalClient.AuditConfigSource(
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
			var propertySourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (propertySourceUpdates.Count > 0)
			{
				var propertySourceToAudit = propertySourceUpdates[0];
				var auditedPropertySource =
					await PortalClient.AuditPropertySource(
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

			var dataSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (dataSourceUpdates.Count > 0)
			{
				await PortalClient
					.ImportLogicModules(
					logicModuleType,
					new List<string>
					{
						dataSourceUpdates[0].Name
					},
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

			var eventSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (eventSourceUpdates.Count > 0)
			{
				await PortalClient
					.ImportLogicModules(
					logicModuleType,
					new List<string>
					{
						eventSourceUpdates[0].Name
					},
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

			var configSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (configSourceUpdates.Count > 0)
			{
				await PortalClient
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

			var propertySourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (propertySourceUpdates.Count > 0)
			{
				await PortalClient
					.ImportLogicModules(
					logicModuleType,
					new List<string>
					{
						propertySourceUpdates[0].Name
					},
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

			var topologySourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (topologySourceUpdates.Count > 0)
			{
				await PortalClient
					.ImportLogicModules(
					logicModuleType,
					new List<string>
					{
						topologySourceUpdates[0].Name
					},
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

			var jobMonitorUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (jobMonitorUpdates.Count > 0)
			{
				await PortalClient
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

			var appliesToFunctionUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (appliesToFunctionUpdates.Count > 0)
			{
				await PortalClient
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

			var snmpSysOidMapUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(logicModuleType, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.New)
				.ToList();

			if (snmpSysOidMapUpdates.Count > 0)
			{
				await PortalClient
					.ImportSnmpSysOidMap(
					new List<SnmpSysOidMapImportItem>
					{
						new SnmpSysOidMapImportItem
						{
							Id = snmpSysOidMapUpdates[0].LocalId,
							Oid = snmpSysOidMapUpdates[0].Name
						}
					},
					default)
					.ConfigureAwait(false);
			}
		}
	}
}