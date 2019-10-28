using LogicMonitor.Api.Flows;
using LogicMonitor.Api.Time;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test
{
	public class FlowTests : TestWithOutput
	{
		public FlowTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
			var endDateTime = DateTime.Today;
			_startDateTimeSeconds = endDateTime.AddDays(-1);
			_endDateTimeSeconds = endDateTime;
		}

		private readonly DateTime _startDateTimeSeconds;
		private readonly DateTime _endDateTimeSeconds;

		//private void CheckExpectedLineCount(GraphData graphData, int expectedLineCount)
		//{
		//	// Make sure that data is returned
		//	Assert.NotNull(graphData);

		//	// Make sure that data is returned
		//	Assert.NotNull(graphData.Lines);

		//	// Make sure that data is returned
		//	Assert.Equal(expectedLineCount, graphData.Lines.Count);
		//}

		[Fact]
		public async void GetApplications()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var flowApplications = await PortalClient.GetFlowApplicationsPageAsync(new FlowApplicationsRequest
			{
				TimePeriod = TimePeriod.OneDay,
				DeviceId = device.Id
			}
			).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flowApplications.Items);
			Assert.True(flowApplications.Items.Count > 0);

			// TODO Make sure that flows are unique in some way
			//Assert.False(flows.Select(flow => flow.Id).HasDuplicates());
		}

		[Fact]
		public async void GetApplicationsForDeviceGroup()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

			var flowApplications = await PortalClient.GetDeviceGroupFlowApplicationsPageAsync(new DeviceGroupFlowApplicationsRequest
			{
				TimePeriod = TimePeriod.Zoom,
				DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",").First()),
				SortDirection = SortDirection.Ascending,
				SortFlowField = FlowField.Usage,
				Take = 100,
				Skip = 0,
				FlowDirection = FlowDirection.All,
				QosType = "all",
				StartDateTime = DateTime.UtcNow.AddDays(-2),
				EndDateTime = DateTime.UtcNow.AddDays(-1),
			})
			.ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flowApplications.Items);
			Assert.True(flowApplications.Items.Count > 0);
		}

		[Fact]
		public async void GetBandwidthsForDeviceGroup()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

			var flowBandwidths = await PortalClient.GetDeviceGroupFlowBandwidthsPageAsync(new DeviceGroupFlowBandwidthsRequest
			{
				TimePeriod = TimePeriod.Zoom,
				DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",").First()),
				SortDirection = SortDirection.Ascending,
				SortFlowField = FlowField.Usage,
				Take = 100,
				Skip = 0,
				FlowDirection = FlowDirection.All,
				StartDateTime = DateTime.UtcNow.AddDays(-2),
				EndDateTime = DateTime.UtcNow.AddDays(-1)
			})
			.ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flowBandwidths.Items);
			Assert.True(flowBandwidths.Items.Count > 0);
		}

		[Fact]
		public async void GetFlowsForDeviceGroup()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

			var flows = await PortalClient.GetDeviceGroupFlowsPageAsync(new DeviceGroupFlowsRequest
			{
				TimePeriod = TimePeriod.Zoom,
				DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",").First()),
				SortDirection = SortDirection.Ascending,
				SortFlowField = FlowField.Usage,
				Take = 100,
				Skip = 0,
				FlowDirection = FlowDirection.All,
				StartDateTime = DateTime.UtcNow.AddDays(-2),
				EndDateTime = DateTime.UtcNow.AddDays(-1)
			})
			.ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flows.Items);
			Assert.True(flows.Items.Count > 0);
		}

		//[Fact]
		//public async void GetDeviceFlowInterfaces()
		//{
		//	var device = await _testPortalConfig.GetNetflowDeviceAsync().ConfigureAwait(false);
		//	var deviceFlowInterfaces =
		//		await _testPortalConfig.PortalClient.GetDeviceFlowInterfacesPageAsync(device.Id,
		//			new Filter<FlowInterface> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		//	// Make sure that some are returned
		//	Assert.NotNull(deviceFlowInterfaces.Items);
		//	Assert.True(deviceFlowInterfaces.Items.Count > 0);

		//	// Make sure that interfaces are unique
		//	Assert.False(deviceFlowInterfaces.Items.Select(flow => flow.Index).HasDuplicates());
		//}

		//[Fact]
		//public async void GetDeviceFlowInterfaces2()
		//{
		//	var testPortalConfig = Master.GetTestPortalConfig();
		//	var device = await testPortalConfig.GetNetflowDeviceAsync().ConfigureAwait(false);
		//	var deviceFlowInterfaces =
		//		await testPortalConfig.PortalClient.GetDeviceFlowInterfacesPageAsync(device.Id,
		//			new Filter<FlowInterface> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		//	// Make sure that some are returned
		//	Assert.NotNull(deviceFlowInterfaces.Items);
		//	Assert.True(deviceFlowInterfaces.Items.Count > 0);

		//	// Make sure that interfaces are unique
		//	Assert.False(deviceFlowInterfaces.Items.Select(flow => flow.Index).HasDuplicates());
		//}

		[Fact]
		public async void GetFlowEndpoints()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var flowEndpoints = await PortalClient.GetFlowEndpointsPageAsync(new FlowEndpointsRequest
			{
				TimePeriod = TimePeriod.OneDay,
				DeviceId = device.Id
			}
			).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flowEndpoints.Items);
			Assert.True(flowEndpoints.Items.Count > 0);

			// TODO Make sure that flows are unique in some way
			//Assert.False(flows.Select(flow => flow.Id).HasDuplicates());
		}

		//[Fact]
		//public async void GetFlowGraphDataAllDirections()
		//{
		//	var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		//	var graphData = await PortalClient.GetGraphDataAsync(new NetflowGraphDataRequest
		//	{
		//		TimePeriod = TimePeriod.Zoom,
		//		StartDateTime = _startDateTimeSeconds,
		//		EndDateTime = _endDateTimeSeconds,
		//		DeviceId = device.Id,
		//	}).ConfigureAwait(false);

		//	// Make sure that data is returned
		//	CheckExpectedLineCount(graphData, 11);
		//}

		//[Fact]
		//public async void GetFlowGraphDataInbound()
		//{
		//	var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		//	var graphData = await PortalClient.GetGraphDataAsync(new NetflowGraphDataRequest
		//	{
		//		TimePeriod = TimePeriod.Zoom,
		//		StartDateTime = _startDateTimeSeconds,
		//		EndDateTime = _endDateTimeSeconds,
		//		DeviceId = device.Id,
		//	}).ConfigureAwait(false);

		//	CheckExpectedLineCount(graphData, 11);

		//	Assert.All(graphData.Lines, line => Assert.False(string.IsNullOrWhiteSpace(line.Legend)));
		//}

		//[Fact]
		//public async void GetFlowGraphDataOutbound()
		//{
		//	var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		//	var graphData = await PortalClient.GetGraphDataAsync(new NetflowGraphDataRequest
		//	{
		//		TimePeriod = TimePeriod.Zoom,
		//		StartDateTime = _startDateTimeSeconds,
		//		EndDateTime = _endDateTimeSeconds,
		//		DeviceId = device.Id,
		//	}).ConfigureAwait(false);

		//	CheckExpectedLineCount(graphData, 11);

		//	Assert.All(graphData.Lines, line => Assert.False(string.IsNullOrWhiteSpace(line.Legend)));
		//}

		[Fact]
		public async void GetFlows()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var flows = await PortalClient.GetFlows(new FlowsRequest
			{
				TimePeriod = TimePeriod.OneDay,
				DeviceId = device.Id
			}
			).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flows.Items);
			Assert.True(flows.Items.Count > 0);

			// TODO Make sure that flows are unique in some way
			//Assert.False(flows.Select(flow => flow.Id).HasDuplicates());
		}

		//[Fact]
		//public async void GetDeviceFlowInterfaceInformation()
		//{
		//	var device = TestPortalConfig.GetNetflowDeviceAsync();
		//	var flowInformation = TestPortalConfig.PortalClient.GetDeviceFlowInformation(device.Id);

		//	// Make sure that interface list is present
		//	Assert.NotNull(flowInformation.Interfaces);

		//	// Make sure that some are returned
		//	Assert.True(flowInformation.Interfaces.Any());

		//	// Make sure that flow interfaces have unique indexes
		//	Assert.False(flowInformation.Interfaces.Select(@interface => @interface.InterfaceIndex).HasDuplicates());
		//}

		[Fact]
		public async void GetPorts()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var flowPorts = await PortalClient.GetFlowPortsPageAsync(new FlowPortsRequest
			{
				TimePeriod = TimePeriod.OneDay,
				DeviceId = device.Id
			}
			).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flowPorts.Items);
			Assert.True(flowPorts.Items.Count > 0);

			// TODO Make sure that flows are unique in some way
			//Assert.False(flows.Select(flow => flow.Id).HasDuplicates());
		}

		[Fact]
		public async void GetZoomTimeFlows()
		{
			var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
			var flows = await PortalClient.GetFlows(new FlowsRequest
			{
				DeviceId = device.Id,
				TimePeriod = TimePeriod.Zoom,
				StartDateTime = _startDateTimeSeconds,
				EndDateTime = _endDateTimeSeconds
			}
			).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.NotNull(flows.Items);
			Assert.True(flows.Items.Count > 0);
		}
	}
}