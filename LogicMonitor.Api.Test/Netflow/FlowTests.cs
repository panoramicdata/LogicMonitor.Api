using System.Globalization;

namespace LogicMonitor.Api.Test.Netflow;

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
	//	graphData.Should().NotBeNull();

	//	// Make sure that data is returned
	//	graphData.Lines.Should().NotBeNull();

	//	// Make sure that data is returned
	//	graphData.Lines.Count.Should().Be(expectedLineCount);
	//}

	[Fact]
	public async void GetApplications()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		var flowApplications = await LogicMonitorClient.GetFlowApplicationsPageAsync(new FlowApplicationsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}
		).ConfigureAwait(false);

		// Make sure that some are returned
		flowApplications.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async void GetApplicationsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

		var flowApplications = await LogicMonitorClient.GetDeviceGroupFlowApplicationsPageAsync(new DeviceGroupFlowApplicationsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
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
		flowApplications.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void GetBandwidthsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

		var flowBandwidths = await LogicMonitorClient.GetDeviceGroupFlowBandwidthsPageAsync(new DeviceGroupFlowBandwidthsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
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
		flowBandwidths.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void GetFlowsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);

		var flows = await LogicMonitorClient.GetDeviceGroupFlowsPageAsync(new DeviceGroupFlowsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
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
		flows.Items.Should().NotBeNullOrEmpty();
	}

	//[Fact]
	//public async void GetDeviceFlowInterfaces()
	//{
	//	var device = await _testPortalConfig.GetNetflowDeviceAsync().ConfigureAwait(false);
	//	var deviceFlowInterfaces =
	//		await _testPortalConfig.PortalClient.GetDeviceFlowInterfacesPageAsync(device.Id,
	//			new Filter<FlowInterface> { Skip = 0, Take = 300 }).ConfigureAwait(false);

	//	// Make sure that some are returned
	//	deviceFlowInterfaces.Items.Should().NotBeNull();
	//	Assert.True(deviceFlowInterfaces.Items.Count > 0);

	//	// Make sure that interfaces are unique
	//	((deviceFlowInterfaces.Items.Select(flow => flow.Index).HasDuplicates())).Should().BeFalse();
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
	//	deviceFlowInterfaces.Items.Should().NotBeNull();
	//	Assert.True(deviceFlowInterfaces.Items.Count > 0);

	//	// Make sure that interfaces are unique
	//	((deviceFlowInterfaces.Items.Select(flow => flow.Index).HasDuplicates())).Should().BeFalse();
	//}

	[Fact]
	public async void GetFlowEndpoints()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		var flowEndpoints = await LogicMonitorClient.GetFlowEndpointsPageAsync(new FlowEndpointsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}
		).ConfigureAwait(false);

		// Make sure that some are returned
		flowEndpoints.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
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

	//	Assert.All(graphData.Lines, line => ((string.IsNullOrWhiteSpace(line.Legend)))).Should().BeFalse();
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

	//	Assert.All(graphData.Lines, line => ((string.IsNullOrWhiteSpace(line.Legend)))).Should().BeFalse();
	//}

	[Fact]
	public async void GetFlows()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		var flows = await LogicMonitorClient.GetFlows(new FlowsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}
		).ConfigureAwait(false);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	//[Fact]
	//public async void GetDeviceFlowInterfaceInformation()
	//{
	//	var device = TestPortalConfig.GetNetflowDeviceAsync();
	//	var flowInformation = TestPortalConfig.PortalClient.GetDeviceFlowInformation(device.Id);

	//	// Make sure that interface list is present
	//	flowInformation.Interfaces.Should().NotBeNull();

	//	// Make sure that some are returned
	//	Assert.True(flowInformation.Interfaces.Any());

	//	// Make sure that flow interfaces have unique indexes
	//	((flowInformation.Interfaces.Select(@interface => @interface.InterfaceIndex).HasDuplicates())).Should().BeFalse();
	//}

	[Fact]
	public async void GetPorts()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		var flowPorts = await LogicMonitorClient.GetFlowPortsPageAsync(new FlowPortsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}
		).ConfigureAwait(false);

		// Make sure that some are returned
		flowPorts.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async void GetZoomTimeFlows()
	{
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		var flows = await LogicMonitorClient.GetFlows(new FlowsRequest
		{
			DeviceId = device.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = _startDateTimeSeconds,
			EndDateTime = _endDateTimeSeconds
		}
		).ConfigureAwait(false);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();
	}
}
