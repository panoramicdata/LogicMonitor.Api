using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///    ConfigSource portal interaction
	/// </summary>
	public partial class PortalClient
	{
		/// <summary>
		///    Gets a ConfigSource by name
		/// </summary>
		/// <param name="configSourceName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[Obsolete("Use GetByNameAsync<ConfigSource> instead")]
		public Task<ConfigSource> GetConfigSourceByNameAsync(string configSourceName, CancellationToken cancellationToken = default)
			=> GetByNameAsync<ConfigSource>(configSourceName, cancellationToken);

		/// <summary>
		///    Get DeviceConfigSources
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="filter">Filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<DeviceConfigSource>> GetDeviceConfigSourcesPageAsync(
			int deviceId,
			Filter<DeviceConfigSource> filter,
			CancellationToken cancellationToken = default)
		{
			filter.FilterItems.Add(new Eq<DeviceConfigSource>(nameof(DeviceConfigSource.DataSourceType), "CS"));
			ValidateFilter(filter);
			return GetBySubUrlAsync<Page<DeviceConfigSource>>($"device/devices/{deviceId}/devicedatasources?{filter}", cancellationToken);
		}

		/// <summary>
		///    Gets devices to which a
		/// </summary>
		/// <param name="configSourceId">The config source id</param>
		/// <param name="filter">Filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<Device>> GetConfigSourceDevicesPageAsync(
			int configSourceId,
			Filter<DeviceConfigSource> filter,
			CancellationToken cancellationToken = default)
		{
			ValidateFilter(filter);
			// setting/configsources/2228820/devices
			return GetBySubUrlAsync<Page<Device>>($"setting/configsources/{configSourceId}/devices?{filter}", cancellationToken);
		}

		/// <summary>
		///    Get DeviceConfigSource
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceConfigSourceId">The device config source id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<DeviceConfigSource> GetDeviceConfigSourceAsync(
			int deviceId,
			int deviceConfigSourceId,
			CancellationToken cancellationToken = default) =>
			// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/88/devicedatasources/36453
			GetBySubUrlAsync<DeviceConfigSource>($"device/devices/{deviceId}/devicedatasources/{deviceConfigSourceId}", cancellationToken);

		/// <summary>
		///    Get DeviceConfigSourceInstances
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceConfigSourceId">The device config source id</param>
		/// <param name="filter">The filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<DeviceConfigSourceInstance>> GetDeviceConfigSourceInstancesPage(
			int deviceId,
			int deviceConfigSourceId,
			Filter<DeviceConfigSourceInstance> filter,
			CancellationToken cancellationToken = default)
		{
			ValidateFilter(filter);
			// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/88/devicedatasources/36453/instances
			return GetBySubUrlAsync<Page<DeviceConfigSourceInstance>>($"device/devices/{deviceId}/devicedatasources/{deviceConfigSourceId}/instances?{filter}", cancellationToken);
		}

		/// <summary>
		///    Get DeviceConfigSourceInstance
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceConfigSourceId">The device config source id</param>
		/// <param name="deviceConfigSourceInstanceId">The device config source instance id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<DeviceConfigSourceInstance> GetDeviceConfigSourceInstanceAsync(
			int deviceId,
			int deviceConfigSourceId,
			int deviceConfigSourceInstanceId,
			CancellationToken cancellationToken = default) =>
			// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/88/devicedatasources/36453/instances/29551739
			GetBySubUrlAsync<DeviceConfigSourceInstance>($"device/devices/{deviceId}/devicedatasources/{deviceConfigSourceId}/instances/{deviceConfigSourceInstanceId}", cancellationToken);

		/// <summary>
		///    Get DeviceConfigSourceInstanceConfig
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceConfigSourceId">The device config source id</param>
		/// <param name="deviceConfigSourceInstanceId">The device config source instance id</param>
		/// <param name="filter">Filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<DeviceConfigSourceInstanceConfig>> GetDeviceConfigSourceInstanceConfigsPageAsync(
			int deviceId,
			int deviceConfigSourceId,
			int deviceConfigSourceInstanceId,
			Filter<DeviceConfigSourceInstanceConfig> filter,
			CancellationToken cancellationToken = default) =>
			// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/66/devicedatasources/36464/instances/29615066/config
			GetBySubUrlAsync<Page<DeviceConfigSourceInstanceConfig>>($"device/devices/{deviceId}/devicedatasources/{deviceConfigSourceId}/instances/{deviceConfigSourceInstanceId}/config?{filter}", cancellationToken);

		/// <summary>
		///    Get DeviceConfigSourceInstanceConfigByIdAndTimestamp
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceConfigSourceId">The device config source id</param>
		/// <param name="deviceConfigSourceInstanceId">The device config source instance id</param>
		/// <param name="id"></param>
		/// <param name="timestamp">The timestamp</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<DeviceConfigSourceInstanceConfig> GetDeviceConfigSourceInstanceConfigByIdAndTimestampAsync(
			int deviceId,
			int deviceConfigSourceId,
			int deviceConfigSourceInstanceId,
			string id,
			long timestamp,
			CancellationToken cancellationToken = default) =>
			// https://panoramicdata.logicmonitor.com/santaba/rest/device/devices/66/devicedatasources/36464/instances/29615066/config/P6CvUmGjTO61a4El8uHATg?startEpoch=1470270899
			GetBySubUrlAsync<DeviceConfigSourceInstanceConfig>($"device/devices/{deviceId}/devicedatasources/{deviceConfigSourceId}/instances/{deviceConfigSourceInstanceId}/config/{id}?startEpoch={timestamp}", cancellationToken);

		/// <summary>
		///    Gets a device config source
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="configSourceId"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<DeviceConfigSource> GetDeviceConfigSourceByDeviceIdAndConfigSourceIdAsync(
			int deviceId,
			int configSourceId,
			CancellationToken cancellationToken = default) =>
			// TODO - permit more than 300
			(await GetDeviceConfigSourcesPageAsync(deviceId, new Filter<DeviceConfigSource> { Skip = 0, Take = 300 }, cancellationToken).ConfigureAwait(false)).Items.SingleOrDefault(deviceConfigSource => deviceConfigSource.ConfigSourceId == configSourceId);
	}
}