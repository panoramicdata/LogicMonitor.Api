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
		public async void GetLogicModuleDataSourceUpdates()
		{
			var dataSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.DataSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(dataSourceUpdates.Items);
		}

		[Fact]
		public async void GetLogicModuleEventSourceUpdates()
		{
			var eventSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.EventSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(eventSourceUpdates.Items);
		}

		[Fact]
		public async void GetLogicModuleConfigSourceUpdates()
		{
			var configSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.ConfigSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(configSourceUpdates.Items);
		}

		[Fact]
		public async void GetLogicModulePropertySourceUpdates()
		{
			var propertySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(propertySourceUpdates.Items);
		}

		[Fact]
		public async void GetLogicModuleTopologySourceUpdates()
		{
			var topologySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.TopologySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(topologySourceUpdates.Items);
		}

		[Fact]
		public async void GetLogicModuleJobMonitorUpdates()
		{
			var jobMonitorUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(jobMonitorUpdates.Items);	// Usually none
		}

		[Fact]
		public async void GetLogicModuleAppliesToUpdates()
		{
			var appliesToUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.AppliesToFunction, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(appliesToUpdates.Items);	// Usually none
		}

		[Fact]
		public async void GetLogicModuleSnmpSysOidUpdates()
		{
			var snmpSysOidUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.SnmpSysOIDMap, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(snmpSysOidUpdates.Items);
		}

		[Fact]
		public async void GetLogicModuleUnknownUpdates()
		{
			var unknownUpdates = new LogicModuleUpdateCollection();

			try
			{
				unknownUpdates =
					await PortalClient
						.GetLogicModuleUpdates(LogicModuleType.Unknown, default)
						.ConfigureAwait(false);
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch
			{
				Assert.True(unknownUpdates.Total == 0);
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
}