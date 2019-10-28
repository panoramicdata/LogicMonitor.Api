using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Data;
using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.Time;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Data
{
	public class DataTests : TestWithOutput
	{
		public DataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetForecastGraphData()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			var dataSourceGraphs = await PortalClient.GetDataSourceGraphsAsync(dataSource.Id).ConfigureAwait(false);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = await PortalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance = deviceDataSourceInstances[0];
			var dataSourceGraph = dataSourceGraphs[0];
			var virtualDataPoint = dataSourceGraph.DataPoints[0];
			var forecastGraphData = await PortalClient.GetForecastGraphDataAsync(new ForecastDataRequest
			{
				TrainingTimePeriod = TrainingTimePeriod.SixMonths,
				ForecastTimePeriod = ForecastTimePeriod.OneMonth,
				DataSourceInstanceId = deviceDataSourceInstance.Id,
				GraphId = dataSourceGraph.Id,
				DataPointLabel = virtualDataPoint.Name
			}).ConfigureAwait(false);
			Assert.Single(forecastGraphData.TrainingGraphData.Lines);
			Assert.Equal(3, forecastGraphData.ForecastedGraphData.Lines.Count);
		}

		[Fact]
		public async void GetOverviewGraphData()
		{
			var device = await GetSnmpDeviceAsync().ConfigureAwait(false);
			Assert.NotNull(device);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("snmp64_If-").ConfigureAwait(false);
			Assert.NotNull(dataSource);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceDataSource);
			var deviceDataSourceInstanceGroups = await PortalClient.GetDeviceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceDataSourceInstanceGroups);
			Assert.NotEmpty(deviceDataSourceInstanceGroups);
			var deviceDataSourceInstanceGroup = deviceDataSourceInstanceGroups.Skip(1).First();
			var deviceDataSourceInstanceGroupRefetch = await PortalClient.GetDeviceDataSourceInstanceGroupByNameAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroup.Name).ConfigureAwait(false);
			Assert.NotNull(deviceDataSourceInstanceGroupRefetch);
			Assert.Equal(deviceDataSourceInstanceGroup.Name, deviceDataSourceInstanceGroupRefetch.Name);

			var overviewGraph = await PortalClient.GetDeviceOverviewGraphByNameAsync(device.Id, deviceDataSource.Id, "Top 10 Interfaces by Total Packets").ConfigureAwait(false);
			Assert.NotNull(overviewGraph);
			var graphDataRequest = new DeviceDataSourceGraphDataRequest
			{
				DataSourceInstanceGroupId = deviceDataSourceInstanceGroup.Id,
				OverviewGraphId = overviewGraph.Id,
				StartDateTime = DateTime.UtcNow.FirstDayOfLastMonth(),
				EndDateTime = DateTime.UtcNow.LastDayOfLastMonth(),
				TimePeriod = TimePeriod.Zoom,
				Width = 500
			};
			graphDataRequest.Validate();
			var graphData = await PortalClient.GetGraphDataAsync(graphDataRequest).ConfigureAwait(false);
			Assert.NotNull(graphData);
		}

		/// <summary>
		/// Netflow Data
		/// </summary>
		[Fact]
		public async void GetNetflowGraphData()
		{
			var utcNow = DateTime.UtcNow;
			var netflowDevice = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var _ = await PortalClient.GetGraphDataAsync(new NetflowGraphDataRequest
			{
				DeviceId = netflowDevice.Id,
				StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
				EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
				NetflowFilter = new NetflowFilter(),
				TimePeriod = TimePeriod.Zoom
			}).ConfigureAwait(false);
		}

		/// <summary>
		/// Netflow Data
		/// </summary>
		[Fact]
		public async void GetNetflowGraphDataForDeviceGroup()
		{
			var utcNow = DateTime.UtcNow;

			// Get the configured Netflow Device
			var netflowDevice = await GetNetflowDeviceAsync().ConfigureAwait(false);

			// Create the request
			var request = new NetflowDeviceGroupGraphDataRequest
			{
				DeviceGroupId = int.Parse(netflowDevice.DeviceGroupIdsString.Split(",").First()),
				StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
				EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
				TimePeriod = TimePeriod.Zoom
			};

			// Send the request
			var data = await PortalClient.GetGraphDataAsync(request).ConfigureAwait(false);

			// Check there is at least one line of data
			Assert.True(data.Lines.Count > 0);
		}

				[Fact]
		public async void GetGraphData_X250()
		{
			PortalClient.UseCache = true;
			var utcNow = DateTime.UtcNow;
			var startDateTime = utcNow.FirstDayOfLastMonth();
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			var dataSourceGraph = await PortalClient.GetDataSourceGraphByNameAsync(dataSource.Id, "CPU Usage").ConfigureAwait(false);
			Assert.NotNull(dataSourceGraph);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = await PortalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>()).ConfigureAwait(false);
			var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
			{
				DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
				DataSourceGraphId = dataSourceGraph.Id,
				TimePeriod = TimePeriod.Zoom,
				StartDateTime = startDateTime,
				EndDateTime = utcNow.LastDayOfLastMonth()
			};
			var sw = Stopwatch.StartNew();
			for (var n = 0; n < 250; n++)
			{
				Logger.LogInformation($"{n:000}: {sw.ElapsedMilliseconds:00000}ms");
				await PortalClient.GetGraphDataAsync(deviceGraphDataRequest).ConfigureAwait(false);
			}
		}

		[Fact]
		public async Task GetGraphData()
		{
			var utcNow = DateTime.UtcNow;
			var startDateTime = utcNow.FirstDayOfLastMonth();
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			Assert.NotNull(device);

			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			Assert.NotNull(dataSource);

			var dataSourceGraph = await PortalClient.GetDataSourceGraphByNameAsync(dataSource.Id, "CPU Usage").ConfigureAwait(false);
			Assert.NotNull(dataSourceGraph);

			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceDataSource);

			var deviceDataSourceInstances = await PortalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>()).ConfigureAwait(false);
			Assert.NotNull(deviceDataSourceInstances);
			Assert.NotEmpty(deviceDataSourceInstances);

			var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
			{
				DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
				DataSourceGraphId = dataSourceGraph.Id,
				TimePeriod = TimePeriod.Zoom,
				StartDateTime = startDateTime,
				EndDateTime = utcNow.LastDayOfLastMonth()
			};
			var graphData = await PortalClient.GetGraphDataAsync(deviceGraphDataRequest).ConfigureAwait(false);
			Assert.NotEmpty(graphData.Lines);
			Assert.Equal(startDateTime, graphData.StartTimeUtc);
			Assert.NotNull(graphData.Lines[0].ColorString);
		}

		[Fact]
		public async Task GetWidgetGraphData()
		{
			var utcNow = DateTime.UtcNow;
			var startDateTime = utcNow.FirstDayOfLastMonth();
			var dashboard = await GetAllWidgetsDashboardAsync().ConfigureAwait(false);
			Assert.NotNull(dashboard);

			var widgets = await PortalClient.GetWidgetsByDashboardIdAsync(dashboard.Id).ConfigureAwait(false);
			Assert.NotNull(widgets);
			Assert.NotEmpty(widgets);

			var firstCustomGraphWidget = widgets.Find(w => w.Type == "cgraph");
			Assert.NotNull(firstCustomGraphWidget);

			var widgetGraphDataRequest = new WidgetGraphDataRequest
			{
				WidgetId = firstCustomGraphWidget.Id,
				TimePeriod = TimePeriod.Zoom,
				StartDateTime = startDateTime,
				EndDateTime = utcNow.LastDayOfLastMonth()
			};
			var graphData = await PortalClient.GetGraphDataAsync(widgetGraphDataRequest).ConfigureAwait(false);
			Assert.NotEmpty(graphData.Lines);
			Assert.Equal(startDateTime, graphData.StartTimeUtc);
			Assert.NotNull(graphData.Lines[0].ColorString);
		}

		[Fact]
		public async void GetWinCpuDeviceDataSourceInstancesFromDev()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = (await PortalClient
					.GetAllDeviceDataSourceInstancesAsync(
						device.Id,
						deviceDataSource.Id,
						new Filter<DeviceDataSourceInstance>
						{
							Take = 300,
							ExtraFilters = new List<FilterItem<DeviceDataSourceInstance>>
							{
								new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
							},
							Order = new Order<DeviceDataSourceInstance> { Property = nameof(DeviceDataSourceInstance.Name), Direction = OrderDirection.Asc }
						}).ConfigureAwait(false));
			Assert.NotNull(deviceDataSourceInstances);
			Assert.NotEmpty(deviceDataSourceInstances);
		}
	}
}