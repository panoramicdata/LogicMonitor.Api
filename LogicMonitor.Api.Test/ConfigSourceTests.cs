using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test;

public class ConfigSourceTests : TestWithOutput
{
	public ConfigSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllConfigSources()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>().ConfigureAwait(false);

		// Make sure that some are returned
		Assert.NotEmpty(configSources);

		// Make sure that all have Unique Ids
		Assert.False(configSources.Select(c => c.Id).HasDuplicates());
	}

	[Fact]
	public async void GetConfigSourceById()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>().ConfigureAwait(false);
		Assert.NotEmpty(configSources);
		var configSource = await LogicMonitorClient.GetAsync<ConfigSource>(configSources[0].Id).ConfigureAwait(false);
		Assert.NotNull(configSource);
	}

	[Fact]
	public async void GetConfigSourceAndAssociatedDevices()
	{
		var configSource = await LogicMonitorClient.GetByNameAsync<ConfigSource>("Cisco_IOS").ConfigureAwait(false);
		Assert.NotNull(configSource);

		// Refetch and check
		var refetch = await LogicMonitorClient.GetAsync<ConfigSource>(configSource.Id).ConfigureAwait(false);
		Assert.Equal("Cisco_IOS", refetch.Name);
		Assert.Equal(configSource.DisplayName, refetch.DisplayName);

		// Get associated devices
		var devices = await LogicMonitorClient.GetConfigSourceDevicesPageAsync(configSource.Id, new Filter<DeviceConfigSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);
		Assert.NotEmpty(devices.Items);
	}

	[Fact]
	public async void GetDeviceConfigSources()
	{
		// NB Limit iterations at each level
		const int maxIterations = 3;

		var portalClient = LogicMonitorClient;
		var device = await GetNetflowDeviceAsync().ConfigureAwait(false);
		Assert.NotNull(device);

		var deviceConfigSources = (await portalClient.GetDeviceConfigSourcesPageAsync(device.Id, new Filter<DeviceConfigSource> { Skip = 0, Take = maxIterations }).ConfigureAwait(false)).Items;
		Assert.NotEmpty(deviceConfigSources);

		foreach (var deviceConfigSource in deviceConfigSources)
		{
			// Get the deviceConfigSource
			var deviceConfigSourceDetails = await portalClient.GetDeviceConfigSourceAsync(device.Id, deviceConfigSource.Id).ConfigureAwait(false);
			Assert.NotNull(deviceConfigSourceDetails);

			// Get the configSourceInstances
			var deviceConfigSourceInstances = await portalClient.GetDeviceConfigSourceInstancesPage(device.Id, deviceConfigSource.Id, new Filter<DeviceConfigSourceInstance> { Skip = 0, Take = maxIterations }).ConfigureAwait(false);
			Assert.NotNull(deviceConfigSourceInstances);
			var configSourceInstances = deviceConfigSourceInstances.Items;
			foreach (var deviceConfigSourceInstance in configSourceInstances)
			{
				// Get the configSourceInstance
				var deviceConfigSourceInstanceDetails = await portalClient.GetDeviceConfigSourceInstanceAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id).ConfigureAwait(false);
				Assert.NotNull(deviceConfigSourceInstanceDetails);

				// Get the latest config for this deviceConfigSourceInstance
				var deviceConfigs = await portalClient.GetDeviceConfigSourceInstanceConfigsPageAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id, new Filter<DeviceConfigSourceInstanceConfig> { Skip = 0, Take = maxIterations }).ConfigureAwait(false);
				Assert.NotNull(deviceConfigs);
				var deviceConfigItems = deviceConfigs.Items;
				// Assert.NotEmpty(deviceConfigItems);
				foreach (var deviceConfig in deviceConfigItems)
				{
					if (deviceConfig.PollTimestampUtc == null)
					{
						throw new Exception("Unexpected lack of timestamp");
					}
					var deviceConfigDetail = await portalClient.GetDeviceConfigSourceInstanceConfigByIdAndTimestampAsync(device.Id, deviceConfigSource.Id, deviceConfigSourceInstance.Id, deviceConfig.Id, deviceConfig.PollTimestampUtc.Value).ConfigureAwait(false);
					Assert.NotNull(deviceConfigDetail);
				}
			}
		}
	}
}
