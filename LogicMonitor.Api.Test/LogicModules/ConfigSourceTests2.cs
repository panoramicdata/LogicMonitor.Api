using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test;

public class ConfigSourceTests2 : TestWithOutput
{
	public ConfigSourceTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllConfigSources()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>().ConfigureAwait(false);

		// Make sure that some are returned
		configSources.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		configSources.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetConfigSourceById()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>().ConfigureAwait(false);
		configSources.Should().NotBeNullOrEmpty();
		var configSource = await LogicMonitorClient.GetAsync<ConfigSource>(configSources[0].Id).ConfigureAwait(false);
		configSource.Should().NotBeNull();
	}

	[Fact]
	public async Task GetConfigSourceAndAssociatedDevices()
	{
		var configSource = await LogicMonitorClient.GetByNameAsync<ConfigSource>("Cisco_IOS").ConfigureAwait(false);
		configSource.Should().NotBeNull();

		// Refetch and check
		var refetch = await LogicMonitorClient.GetAsync<ConfigSource>(configSource.Id).ConfigureAwait(false);
		refetch.Name.Should().Be("Cisco_IOS");
		refetch.DisplayName.Should().Be(configSource.DisplayName);

		// Get associated devices
		var devices = await LogicMonitorClient.GetConfigSourceDevicesPageAsync(configSource.Id, new Filter<DeviceConfigSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);
		devices.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceConfigSources()
	{
		// NB Limit iterations at each level
		const int maxIterations = 3;

		var portalClient = LogicMonitorClient;
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		device.Should().NotBeNull();

		var deviceConfigSources = (await portalClient.GetDeviceConfigSourcesPageAsync(device.Id, new Filter<DeviceConfigSource> { Skip = 0, Take = maxIterations }).ConfigureAwait(false)).Items;
		deviceConfigSources.Should().NotBeNullOrEmpty();

		foreach (var deviceConfigSource in deviceConfigSources)
		{
			// Get the deviceConfigSource
			var deviceConfigSourceDetails = await portalClient.GetDeviceConfigSourceAsync(device.Id, deviceConfigSource.Id).ConfigureAwait(false);
			deviceConfigSourceDetails.Should().NotBeNull();

			// Get the configSourceInstances
			var deviceConfigSourceInstances = await portalClient.GetDeviceConfigSourceInstancesPage(device.Id, deviceConfigSource.Id, new Filter<DeviceConfigSourceInstance> { Skip = 0, Take = maxIterations }).ConfigureAwait(false);
			deviceConfigSourceInstances.Should().NotBeNull();
			var configSourceInstances = deviceConfigSourceInstances.Items;
			foreach (var deviceConfigSourceInstance in configSourceInstances)
			{
				// Get the configSourceInstance
				var deviceConfigSourceInstanceDetails = await portalClient.GetDeviceConfigSourceInstanceAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id).ConfigureAwait(false);
				deviceConfigSourceInstanceDetails.Should().NotBeNull();

				// Get the latest config for this deviceConfigSourceInstance
				var deviceConfigs = await portalClient.GetDeviceConfigSourceInstanceConfigsPageAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id, new Filter<DeviceConfigSourceInstanceConfig> { Skip = 0, Take = maxIterations }).ConfigureAwait(false);
				deviceConfigs.Should().NotBeNull();
				var deviceConfigItems = deviceConfigs.Items;
				// deviceConfigItems.Should().NotBeNullOrEmpty();
				foreach (var deviceConfig in deviceConfigItems)
				{
					if (deviceConfig.PollTimestampUtc is null)
					{
						throw new Exception("Unexpected lack of timestamp");
					}

					var deviceConfigDetail = await portalClient.GetDeviceConfigSourceInstanceConfigByIdAndTimestampAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id, deviceConfig.Id, deviceConfig.PollTimestampUtc.Value).ConfigureAwait(false);
					deviceConfigDetail.Should().NotBeNull();
				}
			}
		}
	}
}
