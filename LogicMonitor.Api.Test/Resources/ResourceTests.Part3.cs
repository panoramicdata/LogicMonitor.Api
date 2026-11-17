using System.Globalization;

namespace LogicMonitor.Api.Test.Resources;

public partial class ResourceTests
{

	[Fact]
	public async Task PatchResourceAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var oldDescription = device.Description;
		var newDescription = Guid.NewGuid().ToString();
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", newDescription } }, CancellationToken);
		var updatedDevice = await GetWindowsResourceAsync(CancellationToken);
		updatedDevice.Description.Should().Be(newDescription);
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", oldDescription } }, CancellationToken);
	}

	[Fact]
	public Task ScheduleActiveDiscovery()
		=> LogicMonitorClient
			.ScheduleActiveDiscovery(WindowsDeviceId, CancellationToken);

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetResourceAlertSettings()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);

		var config = await LogicMonitorClient
			.GetResourceDataPointConfigurationsAsync(device.Id, CancellationToken);

		config.Should().NotBeEmpty();
	}

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetResourceDataSourceInstanceDataPointConfigurationAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);

		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SNMP_Network_Interfaces", CancellationToken);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, CancellationToken);

		var deviceDataSourceInstance = deviceDataSourceInstances.FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(
				device.Id,
				deviceDataSource.Id,
				deviceDataSourceInstance.Id,
				CancellationToken);

		config.Should().NotBeNull();
	}

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetTopTalkerGraphAsync()
	{
		var devicesPage = await LogicMonitorClient
			.GetResourcesPageAsync(new Filter<Resource> { Skip = 0, Take = 50 }, CancellationToken);

		var netflowEnabledId = devicesPage
			.Items
			.FirstOrDefault(device => device.EnableNetflow)?.Id ?? NetflowDeviceId;
		netflowEnabledId.Should().BePositive();

		var graph = await LogicMonitorClient
			.GetTopTalkersGraphAsync(netflowEnabledId, CancellationToken);
		graph.Should().NotBeNull();
	}
}
