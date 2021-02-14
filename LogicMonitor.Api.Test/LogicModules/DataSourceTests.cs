using LogicMonitor.Api.DeviceProcesses;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

// Older, now deprecated methods are still tested here
#pragma warning disable 618

namespace LogicMonitor.Api.Test.LogicModules
{
	public class DataSourceTests : TestWithOutput
	{
		public DataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetDeviceGroupDataSources()
		{
			var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(deviceGroup);

			var deviceGroupDataSources = await LogicMonitorClient.GetAllDeviceGroupDataSourcesAsync(deviceGroup.Id).ConfigureAwait(false);
			Assert.NotEmpty(deviceGroupDataSources);
		}

		[Fact]
		public async void GetDeviceGroupDeviceDataSourceInstances()
		{
			var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(deviceGroup);
			Assert.NotEqual(0, deviceGroup.Id);
			// We have the device group

			// Determine the DataSources
			var dataSourcesIds = new List<DataSource>
				{
					await LogicMonitorClient.GetDataSourceByUniqueNameAsync("Ping").ConfigureAwait(false),
					await LogicMonitorClient.GetDataSourceByUniqueNameAsync("dns").ConfigureAwait(false)
				}
				.ConvertAll(ds => ds.Id);

			var deviceDataSourceInstances = await LogicMonitorClient
				.GetInstancesAsync(LogicModuleType.DataSource, deviceGroup.Id, dataSourcesIds)
				.ConfigureAwait(false);

			Assert.NotNull(deviceDataSourceInstances);
			Assert.NotEmpty(deviceDataSourceInstances);

			var sum = 0;
			foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
			{
				var device = await LogicMonitorClient
					.GetAsync<Device>(deviceDataSourceInstance.DeviceId.Value)
					.ConfigureAwait(false);
				var refetchedDeviceDataSourceInstanceCount = (await LogicMonitorClient
				 .GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(deviceDataSourceInstance.DeviceId.Value, deviceDataSourceInstance.DataSourceId.Value)
				 .ConfigureAwait(false)).InstanceCount;
				Assert.NotEqual(0, refetchedDeviceDataSourceInstanceCount);
				sum += refetchedDeviceDataSourceInstanceCount;
			}
			Assert.Equal(deviceDataSourceInstances.Count, sum);
		}

		//[Fact]
		//public async void GetDeviceGroupDeviceDataSourceInstancesWithRegexFilter()
		//{
		//	var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
		//	Assert.NotNull(deviceGroup);
		//	Assert.NotEqual(0, deviceGroup.Id);
		//	// We have the device group

		//	// Determine the DataSources
		//	var dataSourcesIds = new List<DataSource>
		//		{
		//			await PortalClient.GetDataSourceByUniqueNameAsync("snmp64_If-").ConfigureAwait(false),
		//		}
		//		.Select(ds => ds.Id)
		//		.ToList();

		//	var deviceDataSourceInstances = await PortalClient
		//		.GetInstancesAsync(
		//			LogicModuleType.DataSource,
		//			deviceGroup.Id,
		//			dataSourcesIds,
		//			"port1",
		//			new Regex("^true$"),
		//			new Filter<InstanceProperty> { FilterItems = new List<FilterItem<InstanceProperty>> { new Eq<InstanceProperty>(nameof(InstanceProperty.Name), "port1") } })
		//		.ConfigureAwait(false);

		//	Assert.NotNull(deviceDataSourceInstances);
		//	Assert.NotEmpty(deviceDataSourceInstances);
		//	Assert.Single(deviceDataSourceInstances);
		//}

		[Fact]
		public async void GetWinService()
		{
			var portalClient = LogicMonitorClient;
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var windowsServices = await portalClient.GetDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService).ConfigureAwait(false);
			Assert.NotNull(windowsServices);
			Assert.NotNull(windowsServices.Items);
			Assert.NotEmpty(windowsServices.Items);
		}

		[Fact]
		public async void GetMonitoredWinService()
		{
			var portalClient = LogicMonitorClient;
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var windowsServices = await portalClient.GetMonitoredDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService).ConfigureAwait(false);
			Assert.NotNull(windowsServices);
			Assert.NotNull(windowsServices);
			Assert.NotEmpty(windowsServices);
		}

		[Fact]
		public async void GetXml()
		{
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			var xml = await LogicMonitorClient.GetDataSourceXmlAsync(dataSource.Id).ConfigureAwait(false);

			Assert.NotNull(xml);
		}

		[Fact]
		public async void GetDataSourcesPage()
		{
			var dataSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<DataSource> { Skip = 0, Take = 10 }).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.True(dataSourcePage.Items.Count > 0);

			// Make sure that all have Unique Ids
			Assert.False(dataSourcePage.Items.Select(c => c.Id).HasDuplicates());

			// Check each one
			var dataSourcesString = string.Empty;
			foreach (var dataSource in dataSourcePage.Items)
			{
				// TODO
				//dataSourcesString += $"{dataSource.Name} / {dataSource.DisplayedAs}\r\n";
				//TestOverviewGraphs(dataSource);
				//TestGraphs(dataSource);
			}
			Logger.LogInformation(dataSourcesString);
		}

		[Fact]
		public async void GetAllDataSources()
		{
			var dataSources = await LogicMonitorClient.GetAllAsync<DataSource>().ConfigureAwait(false);
			Assert.NotNull(dataSources);
			Assert.NotEmpty(dataSources);
		}

		[Fact]
		public async void GetDataPointThresholdDetailsForDeviceDataSourceInstance()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = await LogicMonitorClient.GetDeviceDataSourceInstancesPageAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 10 }).ConfigureAwait(false);
			var deviceDataSourceInstance = deviceDataSourceInstances.Items[0];
			var dataPointDetails = await LogicMonitorClient.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);
			var dataPointConfiguration = dataPointDetails.Items[0];
			Assert.NotNull(dataPointConfiguration?.GlobalAlertExpr);
		}

		[Fact]
		public async void GetDataSourceByUniqueName_ValidName_Ok()
		{
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			Assert.NotNull(dataSource);
		}

		[Fact]
		public async void GetDataSourceByUniqueName_ValidNameWithSpaces_Ok()
		{
			const string DataSourceName = "IP Addresses";
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync(DataSourceName).ConfigureAwait(false);
			Assert.NotNull(dataSource);
			Assert.Equal(DataSourceName, dataSource.Name);
		}

		[Fact]
		public async void GetDataSourceByUniqueName_BadName_Null()
		{
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU-").ConfigureAwait(false);
			Assert.Null(dataSource);
		}

		[Fact]
		public async void GetDeviceDataSourceInstances()
		{
			var portalClient = LogicMonitorClient;
			var device = await GetSnmpDeviceAsync().ConfigureAwait(false);
			Assert.NotNull(device);

			var dataSource = await portalClient.GetByNameAsync<DataSource>("snmp64_If-").ConfigureAwait(false);
			Assert.NotNull(dataSource);

			var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceDataSource);

			var deviceDataSourceInstances = (await portalClient
				.GetDeviceDataSourceInstancesPageAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Skip = 0,
						Take = 10,
						Properties = new List<string> { nameof(DeviceDataSourceInstance.Id) }
					})
				.ConfigureAwait(false)).Items;
			Assert.NotNull(deviceDataSourceInstances);
			foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
			{
				Assert.NotNull(deviceDataSourceInstance);
				var deviceDataSourceInstanceRefetch = await portalClient.GetDeviceDataSourceInstanceAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);
				Assert.NotNull(deviceDataSourceInstanceRefetch);

				// Get all instance properties
				var deviceDataSourceInstanceProperties = await portalClient
					.GetDeviceDataSourceInstancePropertiesAsync(
						device.Id,
						deviceDataSource.Id,
						deviceDataSourceInstance.Id,
						new Filter<DeviceDataSourceInstanceProperty>
						{
							Skip = 0,
							Take = 300,
						})
					.ConfigureAwait(false);
				Assert.NotNull(deviceDataSourceInstanceProperties);

				// Check each
				foreach (var deviceDataSourceInstanceProperty in deviceDataSourceInstanceProperties.Items)
				{
					Assert.NotNull(deviceDataSourceInstanceProperty.Name);
					Assert.NotNull(deviceDataSourceInstanceProperty.Value);

					// Refetch it
					var deviceDataSourceInstancePropertyRefetch = await portalClient
						.GetDeviceDataSourceInstancePropertyAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, deviceDataSourceInstanceProperty.Name)
						.ConfigureAwait(false);
					Assert.Equal(deviceDataSourceInstanceProperty.Name, deviceDataSourceInstancePropertyRefetch.Name);
					Assert.Equal(deviceDataSourceInstanceProperty.Value, deviceDataSourceInstancePropertyRefetch.Value);
				}
			}

			var deviceDataSourceInstanceGroups = await portalClient.GetDeviceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceDataSourceInstanceGroups);

			foreach (var deviceDataSourceInstanceGroup in deviceDataSourceInstanceGroups)
			{
				Assert.NotNull(deviceDataSourceInstanceGroup);

				var deviceDataSourceInstanceGroupInstances = await portalClient
					.GetDeviceDataSourceInstanceGroupInstancesPageAsync(
						device.Id,
						deviceDataSource.Id,
						deviceDataSourceInstanceGroup.Id,
						new Filter<DeviceDataSourceInstance>
						{
							Skip = 0,
							Take = 300,
							Properties = new List<string>
							{
								nameof(DeviceDataSourceInstance.Id)
							}
						}).ConfigureAwait(false);
				Assert.NotNull(deviceDataSourceInstanceGroupInstances);
				Assert.NotNull(deviceDataSourceInstanceGroupInstances.Items);

				foreach (var deviceDataSourceInstanceGroupInstance in deviceDataSourceInstanceGroupInstances.Items)
				{
					Assert.NotNull(deviceDataSourceInstanceGroupInstance);
				}
			}
		}

		[Fact]
		public async Task TestDeviceGroupAlertSettings()
		{
			var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			var items = await LogicMonitorClient.GetDeviceGroupDataPointConfigurationAsync(deviceGroup.Id, 3).ConfigureAwait(false);
			Assert.NotNull(items);
		}

		//		private static void TestGraphs(DataSource dataSource)
		//		{
		//// Get graphs
		//			var graphs = await DefaultPortalClient.GetDataSourceGraphList(dataSource.Id);
		//			foreach (var graph in graphs)
		//			{
		//				// Make sure that all have Unique Ids
		//				Assert.False(graphs.Select(c => c.Id).HasDuplicates());

		//				// Get DataPoints
		//				var graphLines = await DefaultPortalClient.GetDataSourceGraphLines(graph.Id);

		//				// Get datapointNames
		//				var graphDataPointNames = await DefaultPortalClient.GetDataSourceGraphDataPointNames(dataSource.Id, graph.Id);

		//				// Make sure that all have Unique Ids
		//				Assert.False(graphDataPointNames.HasDuplicates());

		//				// Get DataPoints
		//				var graphDataPoints = await DefaultPortalClient.GetDataSourceGraphDataPoints(graph.Id);

		//				// Make sure that all have Unique Ids
		//				Assert.False(graphDataPoints.Select(c => c.Id).HasDuplicates());

		//				// Get VirtualDataPoints
		//				var graphVirtualDataPoints = await DefaultPortalClient.GetDataSourceGraphVirtualDataPoints(graph.Id);

		//				// Make sure that all have Unique Ids
		//				Assert.False(graphVirtualDataPoints.Select(c => c.Id).HasDuplicates());
		//			}
		//		}

		//[Fact]
		//private static void TestOverviewGraphs(DataSource dataSource)
		//{
		//	// Get overview graphs
		//	var overviewGraphs = await DefaultPortalClient.GetDataSourceOverviewGraphList(dataSource.Id);
		//	foreach (var overviewGraph in overviewGraphs)
		//	{
		//		// Make sure that all have Unique Ids
		//		Assert.False(overviewGraphs.Select(c => c.Id).HasDuplicates());

		//		// Get datapointNames
		//		var graphDataPointNames = await DefaultPortalClient.GetDataSourceGraphDataPointNames(dataSource.Id, overviewGraph.Id);

		//		// Make sure that all have Unique Ids
		//		Assert.False(graphDataPointNames.HasDuplicates());

		//		//// Get DataPoints
		//		//var graphDataPoints = await DefaultPortalClient.GetDataSourceGraphDataPoints(overviewGraph.Id);

		//		//// Make sure that all have Unique Ids
		//		//Assert.False(graphDataPoints.Select(c => c.Id).HasDuplicates());
		//	}
		//}

		[Fact]
		public async void GetDeviceDataSourceByName_IsFast()
		{
			var stopwatch = Stopwatch.StartNew();
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceDataSources = await LogicMonitorClient.GetDeviceDataSourcesPageAsync(
				device.Id,
				new Filter<DeviceDataSource>
				{
					Take = 1,
					Properties = new List<string>
					{
						nameof(DeviceDataSource.Id),
						nameof(DeviceDataSource.DataSourceName)
					},
					FilterItems = new List<FilterItem<DeviceDataSource>>
					{
						new FilterItem<DeviceDataSource> {
							Property = nameof(DeviceDataSource.DataSourceName),
							Operation = ":",
							Value = "WinCPU"
						}
					}
				}).ConfigureAwait(false);
			var durationMs = stopwatch.ElapsedMilliseconds;

			Assert.NotNull(deviceDataSources);
			var deviceDataSource = deviceDataSources.Items.SingleOrDefault();
			Assert.NotNull(deviceDataSource);
			Assert.True(durationMs < 2000);
		}

		[Fact]
		public async void GetDeviceDataSources()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceDataSources = await LogicMonitorClient.GetDeviceDataSourcesPageAsync(device.Id, new Filter<DeviceDataSource>
			{
				Skip = 0,
				Take = 10,
				Properties = new List<string>
				{
					nameof(DeviceDataSource.Id),
					nameof(DeviceDataSource.CreatedOnSeconds),
				}
			}).ConfigureAwait(false);

			// Make sure that we have groups and they are not null
			Assert.NotNull(deviceDataSources);

			foreach (var deviceDataSource in deviceDataSources.Items)
			{
				// Refetch
				var deviceDataSourceRefetch = await LogicMonitorClient.GetDeviceDataSourceAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);

				// Make sure they are the same
				Assert.Equal(device.Id, deviceDataSourceRefetch.DeviceId);
				Assert.Equal(deviceDataSource.CreatedOnSeconds, deviceDataSourceRefetch.CreatedOnSeconds);

				// Get the instances
				var deviceDataSourceInstances = (await LogicMonitorClient.GetDeviceDataSourceInstancesPageAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Skip = 0,
						Take = 300,
						Properties = new List<string> { nameof(DeviceDataSourceInstance.Id) }
					}).ConfigureAwait(false)).Items;

				// Get the groups
				var deviceDataSourceGroups = await LogicMonitorClient.GetDeviceDataSourceGroupsPageAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceGroup>
					{
						Skip = 0,
						Take = 300,
						Properties = new List<string> { nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DeviceId) }
					}).ConfigureAwait(false);

				// Check any that come back
				foreach (var deviceDataSourceGroup in deviceDataSourceGroups.Items)
				{
					// Make sure they match
					Assert.Equal(device.Id, deviceDataSourceGroup.DeviceId);
				}
			}
		}

		[Fact]
		public async void GetFilteredDataSources()
		{
			const string groupName = ".Net";
			var dataSourcesPage = await LogicMonitorClient.GetAllAsync(new Filter<DataSource>
			{
				FilterItems = new List<FilterItem<DataSource>>
				{
					new Eq<DataSource>(nameof(DataSource.Group), groupName)
				}
			}).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(dataSourcesPage);
			Assert.NotEmpty(dataSourcesPage);
			Assert.True(dataSourcesPage.Count < 20);

			// Make sure that they match the expected group
			Assert.True(dataSourcesPage.All(item => item.Group == groupName));

			// The whole thing should take less than 60 seconds
			AssertIsFast(80);
		}

		[Fact]
		public async void GetDataSourceGroupsQuickly()
		{
			// Get all DataSourceGroups
			var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.Group) } }).ConfigureAwait(false);

			var distinctGroups = dataSources.Select(ds => ds.Group).Distinct().ToList();

			Assert.True(distinctGroups.Count > 1);
		}

		[Fact]
		public async void GetDataSourceCollectionMethodsQuickly()
		{
			// Get all DataSource Collection methods
			var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.CollectionMethod) } }).ConfigureAwait(false);
			Assert.NotNull(dataSources);
		}

		[Fact]
		public async void WindowsServerDisks()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-").ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			Assert.Equal(device.Id, deviceDataSource.DeviceId);
			Assert.Equal(dataSource.Id, deviceDataSource.DataSourceId);
		}
	}
}

#pragma warning restore 618