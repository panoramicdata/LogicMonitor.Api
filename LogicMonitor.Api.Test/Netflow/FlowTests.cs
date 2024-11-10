using System.Globalization;

namespace LogicMonitor.Api.Test.Netflow;

public class FlowTests : TestWithOutput
{
	public FlowTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : base(iTestOutputHelper, fixture)
	{
		var endDateTime = DateTime.Today;
		_startDateTimeSeconds = endDateTime.AddDays(-1);
		_endDateTimeSeconds = endDateTime;
	}

	private readonly DateTime _startDateTimeSeconds;
	private readonly DateTime _endDateTimeSeconds;

	[Fact]
	public async Task GetApplications()
	{
		var flowApplications = await LogicMonitorClient
			.GetFlowApplicationsPageAsync(
				new FlowApplicationsRequest
				{
					ResourceId = NetflowDeviceId
				},
				default
			);

		// Make sure that some are returned
		flowApplications.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetApplicationsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default);

		var flowApplications = await LogicMonitorClient
			.GetResourceGroupFlowApplicationsPageAsync(
				new ResourceGroupFlowApplicationsRequest
				{
					TimePeriod = TimePeriod.Zoom,
					ResourceGroupId = int.Parse(device.ResourceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
					SortDirection = SortDirection.Ascending,
					SortFlowField = FlowField.Usage,
					Take = 100,
					Skip = 0,
					FlowDirection = FlowDirection.All,
					QosType = "all",
					StartDateTime = DateTime.UtcNow.AddDays(-2),
					EndDateTime = DateTime.UtcNow.AddDays(-1),
				},
				default);

		// Make sure that some are returned
		flowApplications.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetBandwidthsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default);

		var flowBandwidths = await LogicMonitorClient.GetResourceGroupFlowBandwidthsPageAsync(new ResourceGroupFlowBandwidthsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			ResourceGroupId = int.Parse(device.ResourceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			SortDirection = SortDirection.Ascending,
			SortFlowField = FlowField.Usage,
			Take = 100,
			Skip = 0,
			FlowDirection = FlowDirection.All,
			StartDateTime = DateTime.UtcNow.AddDays(-2),
			EndDateTime = DateTime.UtcNow.AddDays(-1)
		}, default);

		// Make sure that some are returned
		flowBandwidths.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFlowsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default);

		var flows = await LogicMonitorClient.GetResourceGroupFlowsPageAsync(new ResourceGroupFlowsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			ResourceGroupId = int.Parse(device.ResourceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			SortDirection = SortDirection.Ascending,
			SortFlowField = FlowField.Usage,
			Take = 100,
			Skip = 0,
			FlowDirection = FlowDirection.All,
			StartDateTime = DateTime.UtcNow.AddDays(-2),
			EndDateTime = DateTime.UtcNow.AddDays(-1)
		}, default);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFlowApplications()
	{
		var flowEndpoints = await LogicMonitorClient.GetFlowApplicationsPageAsync(new FlowApplicationsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			ResourceId = NetflowDeviceId
		}, default);

		// Make sure that some are returned
		flowEndpoints.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetFlows()
	{
		var flows = await LogicMonitorClient.GetFlowsPageAsync(new FlowsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			ResourceId = NetflowDeviceId
		}, default);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetPorts()
	{
		var flowPorts = await LogicMonitorClient.GetFlowPortsPageAsync(new FlowPortsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			ResourceId = NetflowDeviceId
		}, default);

		// Make sure that some are returned
		flowPorts.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetZoomTimeFlows()
	{
		var flows = await LogicMonitorClient.GetFlowsPageAsync(new FlowsRequest
		{
			ResourceId = NetflowDeviceId,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = _startDateTimeSeconds,
			EndDateTime = _endDateTimeSeconds
		}, default);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceFlowInformation()
	{
		var interfaces = await LogicMonitorClient
			.GetDeviceFlowInterfacesPageAsync(NetflowDeviceId, new Filter<FlowInterface>(), default);
		interfaces.Items.Should().NotBeNullOrEmpty();
	}
}
