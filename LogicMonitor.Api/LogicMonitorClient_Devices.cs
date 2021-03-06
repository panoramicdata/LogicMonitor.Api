using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.DeviceProcesses;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///     Device Portal interaction
	/// </summary>
	public partial class LogicMonitorClient
	{
		/// <summary>
		///     Gets devices by HostName
		/// </summary>
		/// <param name="deviceName">The Device HostName</param>
		/// <param name="maxResultCount">Max result count.  May not exceed 100</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<List<Device>> GetDevicesByHostNameAsync(string deviceName, int maxResultCount, CancellationToken cancellationToken = default)
			=> (await GetDevicesByNameAsync(deviceName, maxResultCount, cancellationToken).ConfigureAwait(false)).Where(d => string.Equals(d.Name, deviceName, StringComparison.InvariantCultureIgnoreCase)).ToList();

		/// <summary>
		///     Gets device by DisplayName
		/// </summary>
		/// <param name="displayName">The Device DisplayName</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<Device> GetDeviceByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default)
			=> (await GetAllAsync(new Filter<Device>
			{
				FilterItems = new List<FilterItem<Device>>
				{
					new Eq<Device>(nameof(Device.DisplayName), displayName.EscapeSlashes().EscapePlusCharacter())
				}
			}, cancellationToken)
			.ConfigureAwait(false))
			.SingleOrDefault();

		/// <summary>
		///     Get device properties, in the following order:
		///     - Custom
		///     - Auto
		///     - System
		///     - Inherit
		/// </summary>
		/// <param name="deviceId">The Device Id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<List<Property>> GetDevicePropertiesAsync(int deviceId, CancellationToken cancellationToken = default)
			=> GetAllAsync<Property>($"device/devices/{deviceId}/properties", cancellationToken);

		/// <summary>
		///     Get device properties
		/// </summary>
		/// <param name="deviceId">The Device Id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task ScheduleActiveDiscovery(int deviceId, CancellationToken cancellationToken = default)
			=> await PostAsync<object, object>(new object(), $"device/devices/{deviceId}/scheduleAutoDiscovery", cancellationToken).ConfigureAwait(false);

		/// <summary>
		///     Get device alerts
		/// </summary>
		/// <param name="deviceId">The Device Id</param>
		/// <param name="skip">The number to skip</param>
		/// <param name="take">The number to take</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<Alert>> GetDeviceAlertsPageAsync(int deviceId, int skip = 0, int take = 100, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<Alert>>($"device/devices/{deviceId}/alerts?size={take}&offset={skip}", cancellationToken);

		/// <summary>
		///     Set single device property
		/// </summary>
		/// <param name="deviceId">The Device Id</param>
		/// <param name="name">The property name</param>
		/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
		/// <param name="mode">How to set the property.
		/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
		/// If you know that the property is already set and you want to change the value, use Update.
		/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
		/// If you know that the property is NOT already set and you want to set the value, use Create.
		/// </param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task SetDeviceCustomPropertyAsync(
			int deviceId,
			string name,
			string value,
			SetPropertyMode mode = SetPropertyMode.Automatic,
			CancellationToken cancellationToken = default)
		=>
			SetCustomPropertyAsync(
				deviceId,
				name,
				value,
				mode,
				"device/devices",
				cancellationToken
				);

		/// <summary>
		///     Set single deviceGroup property
		/// </summary>
		/// <param name="deviceGroupId">The DeviceGroup Id</param>
		/// <param name="name">The property name</param>
		/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
		/// <param name="mode">How to set the property.
		/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
		/// If you know that the property is already set and you want to change the value, use Update.
		/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
		/// If you know that the property is NOT already set and you want to set the value, use Create.
		/// </param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task SetDeviceGroupCustomPropertyAsync(
			int deviceGroupId,
			string name,
			string value,
			SetPropertyMode mode = SetPropertyMode.Automatic,
			CancellationToken cancellationToken = default)
		=>
			SetCustomPropertyAsync(
				deviceGroupId,
				name,
				value,
				mode,
				"device/groups",
				cancellationToken);

		/// <summary>
		///     Gets devices
		/// </summary>
		/// <param name="filter">The device filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<Device>> GetDevicesPageAsync(Filter<Device> filter, CancellationToken cancellationToken = default)
		{
			if (filter != null && filter.Order == null)
			{
				filter.Order = new Order<Device> { Property = nameof(Device.Id), Direction = OrderDirection.Asc };
			}
			return GetBySubUrlAsync<Page<Device>>($"device/devices?{filter}", cancellationToken);
		}

		/// <summary>
		///     Gets devices by DeviceGroup Id
		/// </summary>
		/// <param name="deviceGroupId"></param>
		/// <param name="filter">The filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<List<Device>> GetDevicesByDeviceGroupIdAsync(int deviceGroupId, Filter<Device> filter, CancellationToken cancellationToken = default)
			=> await GetAllAsync(filter, $"device/groups/{deviceGroupId}/devices", cancellationToken).ConfigureAwait(false);

		/// <summary>
		///     Gets devices by DeviceGroup full path
		/// </summary>
		/// <param name="deviceGroupFullPaths">The FullPath(es) of the DeviceGroup(s), semicolon separated.</param>
		/// <param name="recurse">If true, finds devices in child groups also.</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns>A list of Devices</returns>
		public async Task<List<Device>> GetDevicesByDeviceGroupFullPathAsync(string deviceGroupFullPaths, bool recurse, CancellationToken cancellationToken = default)
		{
			var devices = new List<Device>();

			foreach (var searchDeviceGroupName in deviceGroupFullPaths.Split(';').Select(path => path.TrimStart()).ToList())
			{
				// Make sure the Device Group exists
				if (await GetDeviceGroupByFullPathAsync(searchDeviceGroupName, cancellationToken).ConfigureAwait(false)
					is DeviceGroup checkedDeviceGroup)
				{
					List<DeviceGroup> deviceGroups;

					// The root
					if (checkedDeviceGroup.Id == 1)
					{
						deviceGroups =
							recurse
								// All Device Groups
								? await GetAllAsync<DeviceGroup>(cancellationToken: cancellationToken)
									.ConfigureAwait(false)
								// Only the root
								: await GetAllAsync(
									new Filter<DeviceGroup>
									{
										FilterItems = new List<FilterItem<DeviceGroup>>
										{
											new Eq<DeviceGroup>(nameof(DeviceGroup.Id), 1)
										}
									},
									cancellationToken: cancellationToken)
								.ConfigureAwait(false);
					}
					else
					{
						deviceGroups =
							recurse
								? await GetAllAsync(
									new Filter<DeviceGroup>
									{
										FilterItems = new List<FilterItem<DeviceGroup>>
										{
											new Includes<DeviceGroup>(nameof(DeviceGroup.FullPath), checkedDeviceGroup.FullPath)
										}
									},
									cancellationToken: cancellationToken)
								.ConfigureAwait(false)
								: await GetAllAsync(
									new Filter<DeviceGroup>
									{
										FilterItems = new List<FilterItem<DeviceGroup>>
										{
											new Eq<DeviceGroup>(nameof(DeviceGroup.FullPath), searchDeviceGroupName)
										}
									},
									cancellationToken: cancellationToken)
								.ConfigureAwait(false);
					}

					// Ensure the one we actually found is included
					if (deviceGroups.Any(dg => dg.Id != checkedDeviceGroup.Id))
					{
						deviceGroups.Add(checkedDeviceGroup);
					}

					if (recurse)
					{
						if (checkedDeviceGroup.Id != 1)	// Not the root
						{
							// Filter out the ones where the full path did not START with the searched-for group, as we could
							// only use a Includes<DeviceGroup> filter and not a StartsWith (there isn't one!)
							deviceGroups.RemoveAll(dg => !dg.FullPath.StartsWith(searchDeviceGroupName));
						}
					}

					// Get the Devices
					foreach (var deviceGroup in deviceGroups)
					{
						devices.AddRange(await GetDevicesByDeviceGroupIdAsync(deviceGroup.Id, new Filter<Device> { Skip = 0, Take = 300 }).ConfigureAwait(false));
					}
				}
			}
			return devices
				.DistinctBy(d => d.Id)
				.ToList();
		}

		/// <summary>
		///     Get devices by associated datasource
		/// </summary>
		/// <param name="dataSourceId">The DataSource Id</param>
		/// <param name="filter">The filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<DeviceWithDataSourceInstanceInformation>> GetDevicesAndInstancesAssociatedWithDataSourceByIdPageAsync(int dataSourceId, Filter<DeviceWithDataSourceInstanceInformation> filter, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<DeviceWithDataSourceInstanceInformation>>($"setting/datasources/{dataSourceId}/devices?{filter}", cancellationToken);

		/// <summary>
		///     Get devices by name
		/// </summary>
		/// <param name="searchString">The search string</param>
		/// <param name="maxResultCount">Max result count.  May not exceed 100</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task<List<Device>> GetDevicesByNameAsync(string searchString, int maxResultCount, CancellationToken cancellationToken = default)
		{
			if (searchString == null)
			{
				throw new ArgumentNullException(nameof(searchString));
			}

			var treeNodeFreeSearchResults = await TreeNodeFreeSearchAsync(searchString, maxResultCount, TreeNodeFreeSearchResultType.Device, cancellationToken).ConfigureAwait(false);
			var deviceList = new List<Device>();
			foreach (var deviceResult in treeNodeFreeSearchResults)
			{
				deviceList.Add(await GetAsync<Device>(deviceResult.EntityId, cancellationToken).ConfigureAwait(false));
			}
			return deviceList;
		}

		/// <summary>
		///     Gets devices by DeviceGroup full path
		/// </summary>
		/// <param name="deviceGroupFullPath"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<DeviceGroup> GetDeviceGroupByFullPathAsync(string deviceGroupFullPath, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(deviceGroupFullPath) || deviceGroupFullPath == "/")
			{
				return await GetAsync<DeviceGroup>(1, cancellationToken).ConfigureAwait(false);
			}

			// This may actually return more than one, as LogicMonitor does not correctly handle brackets in some cases.
			var allDeviceGroups = await GetAllAsync(new Filter<DeviceGroup>
			{
				FilterItems = new List<FilterItem<DeviceGroup>>
				{
					new Eq<DeviceGroup>(nameof(DeviceGroup.FullPath), deviceGroupFullPath.EscapePlusCharacter())
				}
			},
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

			return allDeviceGroups.SingleOrDefault(dg => dg.FullPath == deviceGroupFullPath);
		}

		/// <summary>
		///     Gets device Group properties
		/// </summary>
		/// <param name="deviceGroupId"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<List<Property>> GetDeviceGroupPropertiesAsync(int deviceGroupId, CancellationToken cancellationToken = default)
			=> GetAllAsync<Property>($"device/groups/{deviceGroupId}/properties", cancellationToken);

		/// <summary>
		///     Tree node free search
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="maxResultCount"></param>
		/// <param name="treeNodeFreeSearchResultType"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<List<TreeNodeFreeSearchResult>> TreeNodeFreeSearchAsync(
			string searchText,
			int maxResultCount,
			TreeNodeFreeSearchResultType? treeNodeFreeSearchResultType = null,
			CancellationToken cancellationToken = default)
		{
			var modifier = treeNodeFreeSearchResultType switch
			{
				TreeNodeFreeSearchResultType.DeviceDataSourceInstance => "i:",
				TreeNodeFreeSearchResultType.DeviceDataSource => "ds:",
				TreeNodeFreeSearchResultType.Device => "d:",
				TreeNodeFreeSearchResultType.DeviceGroup => "g:",
				null => string.Empty,
				_ => throw new ArgumentException(),
			};
			var treeNodeFreeSearchRequest = new TreeNodeFreeSearchRequest
			{
				Type = TreeNodeFreeSearchRequestType.TreeNodeFreeSearch,
				SearchText = $"{modifier}\"{searchText.EscapeProblematicCharacters()}\"",
				ResultLimitation = maxResultCount
			};

			return (await PostAsync<TreeNodeFreeSearchRequest, Page<TreeNodeFreeSearchResult>>(treeNodeFreeSearchRequest, "functions", cancellationToken).ConfigureAwait(false)).Items;
		}

		/// <summary>
		///     Gets the full device tree
		/// </summary>
		/// <param name="deviceGroupId">The device group id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<DeviceGroup> GetFullDeviceTreeAsync(int deviceGroupId = -1, CancellationToken cancellationToken = default)
		{
			var allDeviceGroups = await GetAllAsync<DeviceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
			var requestedRootGroup = allDeviceGroups.SingleOrDefault(g => g.Id == deviceGroupId) ?? new DeviceGroup { Name = "Root", Id = -1 };
			if (deviceGroupId != requestedRootGroup.Id)
			{
				throw new ArgumentOutOfRangeException($"No deviceGroup found with id {deviceGroupId}");
			}

			foreach (var deviceGroup in allDeviceGroups)
			{
				var parentGroup = allDeviceGroups.SingleOrDefault(g => g.Id == deviceGroup.ParentId) ?? (deviceGroupId == -1 ? requestedRootGroup : null);
				if (parentGroup == null)
				{
					continue;
				}

				if (parentGroup.SubGroups == null)
				{
					parentGroup.SubGroups = new List<DeviceGroup> { deviceGroup };
				}
				else
				{
					parentGroup.SubGroups.Add(deviceGroup);
				}

				var detailedDeviceGroup = await GetAsync<DeviceGroup>(deviceGroup.Id).ConfigureAwait(false);

				deviceGroup.AlertStatus = detailedDeviceGroup.AlertStatus;
			}

			foreach (var device in await GetAllAsync<Device>(cancellationToken: cancellationToken).ConfigureAwait(false))
			{
				foreach (var deviceGroup in device
					.DeviceGroupIdsString
					.Split(',')
					.Select(int.Parse)
					.Select(dgId => allDeviceGroups.SingleOrDefault(g => g.Id == dgId)))
				{
					// Avoids a race condition
					if (deviceGroup == null)
					{
						continue;
					}
					(deviceGroup.Devices ??= new List<Device>()).Add(device);
					deviceGroup.DeviceCount = deviceGroup.Devices.Count;
				}
			}
			return requestedRootGroup;
		}

		/// <summary>
		///     Gets a list of processes being monitored for a device
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceProcessServiceTaskType">The process/service type</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<List<DeviceDataSourceInstance>> GetMonitoredDeviceProcesses(
			int deviceId,
			DeviceProcessServiceTaskType deviceProcessServiceTaskType,
			CancellationToken cancellationToken = default)
		{
			var dataSourceName = deviceProcessServiceTaskType switch
			{
				DeviceProcessServiceTaskType.LinuxProcess => "LinuxNewProcesses-",
				DeviceProcessServiceTaskType.WindowsProcess => "WinProcessStats-",
				DeviceProcessServiceTaskType.WindowsService => "WinService-",
				_ => throw new ArgumentException($"Only {DeviceProcessServiceTaskType.LinuxProcess}, {DeviceProcessServiceTaskType.WindowsProcess} and {DeviceProcessServiceTaskType.WindowsService} are supported", nameof(deviceProcessServiceTaskType)),
			};
			var filter = new Filter<DeviceDataSource>
			{
				FilterItems = new List<FilterItem<DeviceDataSource>>
				{
					new Eq<DeviceDataSource>(nameof(DeviceDataSource.DataSourceName), dataSourceName),
					new Eq<DeviceDataSource>(nameof(DeviceDataSource.DataSourceType), "DS")
				}
			};
			var deviceDataSources = await GetAllAsync(filter, $"device/devices/{deviceId}/devicedatasources", cancellationToken).ConfigureAwait(false);

			if (deviceDataSources.Count != 1)
			{
				return new List<DeviceDataSourceInstance>();
			}

			return await GetAllDeviceDataSourceInstancesAsync(deviceId, deviceDataSources.Single().Id, cancellationToken: cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		///     Initiates a task to fetch the list of current running processes on a device
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceProcessServiceTaskType">The device process/service task type</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<DeviceProcessServiceTask> GetProcessServiceTaskForDevice(
			int deviceId,
			DeviceProcessServiceTaskType deviceProcessServiceTaskType,
			CancellationToken cancellationToken = default)
			=> PostAsync<DeviceProcessServiceTask, DeviceProcessServiceTask>(
				new DeviceProcessServiceTask
				{
					Type = deviceProcessServiceTaskType
				},
				$"device/devices/{deviceId}/fetchProcessServiceTask",
				cancellationToken
			);

		/// <summary>
		/// For getting process results
		/// </summary>
		[DataContract]
		private class ProcessServiceTaskResultError
		{
			[DataMember(Name = "errorCode")]
			public int ErrorCode { get; set; }

			[DataMember(Name = "errorMessage")]
			public string ErrorMessage { get; set; }

			[DataMember(Name = "errorDetail")]
			public object ErrorDetail { get; set; }
		}

		/// <summary>
		///     Fetches a task result
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="taskId">The task id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns>The task results</returns>
		public async Task<Page<DeviceProcess>> GetProcessServiceTaskResults(int deviceId, int taskId, CancellationToken cancellationToken = default)
		{
			while (true)
			{
				var jObject = await GetBySubUrlAsync<JObject>($"device/devices/{deviceId}/fetchProcessServiceTask/{taskId}", cancellationToken).ConfigureAwait(false);
				var error = jObject.ToObject<ProcessServiceTaskResultError>();
				if (error.ErrorMessage == null)
				{
					return jObject.ToObject<Page<DeviceProcess>>();
				}
				await Task.Delay(500).ConfigureAwait(false);
			}
		}

		/// <summary>
		///     Gets currently-running device processes
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceProcessServiceTaskType">The process/service type</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<Page<DeviceProcess>> GetDeviceProcesses(
			int deviceId,
			DeviceProcessServiceTaskType deviceProcessServiceTaskType,
			CancellationToken cancellationToken = default)
		{
			var task = await GetProcessServiceTaskForDevice(deviceId, deviceProcessServiceTaskType, cancellationToken).ConfigureAwait(false);
			return await GetProcessServiceTaskResults(deviceId, task.TaskId, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Gets a DeviceDataSourceInstance's DataPointConfigurations
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="deviceDataSourceId"></param>
		/// <param name="deviceDataSourceInstanceId"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<List<DataPointConfiguration>> GetDeviceDataSourceInstanceDataPointConfigurations(
			int deviceId,
			int deviceDataSourceId,
			int deviceDataSourceInstanceId,
			CancellationToken cancellationToken = default)
			=> (await GetAsync<Page<DataPointConfiguration>>(
				false,
				$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/alertsettings?offset=0&size=300",
				cancellationToken).ConfigureAwait(false)).Items;

		/// <summary>
		/// Gets a Device's DataPointConfigurations
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<List<DataPointConfiguration>> GetDeviceDataPointConfigurations(
			int deviceId,
			CancellationToken cancellationToken = default)
			=> await GetAllAsync<DataPointConfiguration>(
				$"device/devices/{deviceId}/alertsettings",
				cancellationToken).ConfigureAwait(false);

		/// <summary>
		///     Update a DataPointConfiguration
		/// </summary>
		/// <param name="deviceId">The Device Id</param>
		/// <param name="deviceDataSourceId">The DeviceDataSource Id</param>
		/// <param name="deviceDataSourceInstanceId">The DeviceDataSourceInstance Id</param>
		/// <param name="dataPointConfiguration">The DataPointConfiguration</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task UpdateDataPointConfiguration(
			int deviceId,
			int deviceDataSourceId,
			int deviceDataSourceInstanceId,
			DataPointConfiguration dataPointConfiguration,
			CancellationToken cancellationToken = default)
			=> await PutAsync(
				$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/alertsettings/{dataPointConfiguration.Id}",
				dataPointConfiguration,
				cancellationToken).ConfigureAwait(false);

		/// <summary>
		/// Sets alert thresholds for an entire device datasource instance group
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="deviceDataSourceId">The DeviceDataSource id</param>
		/// <param name="deviceDataSourceInstanceGroupId">The deviceDataSource Instance Group Id (0 == default)</param>
		/// <param name="dataPointId">The DataPoint Id</param>
		/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
		/// <param name="alertExpressionNote">A note explaining the threshold</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task SetDeviceDataSourceInstanceGroupDataPointThresholds(
			int deviceId,
			int deviceDataSourceId,
			int deviceDataSourceInstanceGroupId,
			int dataPointId,
			string alertExpression,
			string alertExpressionNote,
			CancellationToken cancellationToken = default)
			=> await PutAsync($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups/{deviceDataSourceInstanceGroupId}/datapoints/{dataPointId}/alertconfig",
				new ThresholdSpecification
				{
					AlertExpression = alertExpression,
					AlertExpressionNote = alertExpressionNote
				}, cancellationToken).ConfigureAwait(false);

		/// <summary>
		/// Sets alert thresholds for an entire device datasource instance group
		/// </summary>
		/// <param name="deviceGroupId">The device id</param>
		/// <param name="dataSourceId">The DeviceDataSource id</param>
		/// <param name="dataPointId">The DataPoint Id</param>
		/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
		/// <param name="alertExpressionNote">A note explaining the threshold</param>
		/// <param name="disableAlerting">Disable alerting</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task SetDeviceGroupDataSourceDataPointThresholds(
			int deviceGroupId,
			int dataSourceId,
			int dataPointId,
			string alertExpression,
			string alertExpressionNote,
			bool? disableAlerting,
			CancellationToken cancellationToken = default)
		{
			// Need to get the existing Alert config, modify it and then PUT it back
			var url = $"device/groups/{deviceGroupId}/datasources/{dataSourceId}/alertsettings";

			var dataPointConfigurationCollection = await GetBySubUrlAsync<DataPointConfigurationCollection>($"device/groups/{deviceGroupId}/datasources/{dataSourceId}/alertsettings", cancellationToken).ConfigureAwait(false);

			var changeMade = false;

			foreach (var dpConfig in dataPointConfigurationCollection.Items)
			{
				if (dpConfig.DataPointId == dataPointId)
				{
					if (alertExpression != null && dpConfig.AlertExpression != alertExpression)
					{
						dpConfig.AlertExpression = alertExpression;
						changeMade = true;
					}
					if (alertExpressionNote != null && dpConfig.AlertExpressionNote != alertExpressionNote)
					{
						dpConfig.AlertExpressionNote = alertExpressionNote;
						changeMade = true;
					}
					if (disableAlerting.HasValue && dpConfig.DisableAlerting != disableAlerting)
					{
						dpConfig.DisableAlerting = disableAlerting.Value;
						changeMade = true;
					}
				}
			}
			if (changeMade)
			{
				await PutAsync(url, dataPointConfigurationCollection, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <summary>
		///     Gets a list of DataPointThresholdDetails
		/// </summary>
		/// <param name="deviceGroupId">The device group Id</param>
		/// <param name="dataSourceId">The dataSource Id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public async Task<List<DataPointConfiguration>> GetDeviceGroupDataPointConfigurationAsync(
			int deviceGroupId,
			int dataSourceId,
			CancellationToken cancellationToken = default)
			=> (await GetBySubUrlAsync<DataPointConfigurationCollection>($"device/groups/{deviceGroupId}/datasources/{dataSourceId}/alertsettings", cancellationToken).ConfigureAwait(false)).Items;

		/// <summary>
		/// Gets all device instances matching the provided filter
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="filter">The optional filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<List<DeviceDataSourceInstance>> GetAllDeviceInstances(int deviceId, Filter<DeviceDataSourceInstance> filter, CancellationToken cancellationToken)
			=> GetAllInternalAsync(filter, $"device/devices/{deviceId}/instances", cancellationToken);

		/// <summary>
		/// Get Alerts for a Device by ID
		/// </summary>
		/// <param name="deviceId">The Device ID</param>
		/// <param name="filter">The Alert Filter</param>
		/// <param name="cancellationToken">The CancellationToken</param>
		/// <returns>A List of Alerts</returns>
		public async Task<List<Alert>> GetDeviceAlertsByIdAsync(
			int deviceId,
			AlertFilter filter,
			CancellationToken cancellationToken = default)
		{
			// Ensure that the filter CANNOT override the device ID set!
			filter.RemoveMonitorObjectReferences();

			var (alerts, limitReached) = await
				GetDeviceAlertsByIdNormalAsync(deviceId, filter, false, cancellationToken)
				.ConfigureAwait(false);

			if (limitReached)
			{
				// Fall back to the chunked method
				alerts = await GetDeviceAlertsByIdChunkedAsync(deviceId, filter, TimeSpan.FromHours(24)).ConfigureAwait(false);
			}

			if (filter?.IsCleared == true)
			{
				return alerts.Where(a => a.IsCleared).ToList();
			}
			if (filter?.IsCleared == false)
			{
				return alerts.Where(a => !a.IsCleared).ToList();
			}
			return alerts;
		}

		/// <summary>
		///     This version of the call requests hourly chunks
		/// </summary>
		/// <param name="deviceId">The Device ID</param>
		/// <param name="filter"></param>
		/// <param name="chunkSize">The chunk size (TimeSpan)</param>
		internal async Task<List<Alert>> GetDeviceAlertsByIdChunkedAsync(
			int deviceId,
			AlertFilter filter,
			TimeSpan chunkSize)
		{
			// Ensure that the filter CANNOT override the device ID set!
			filter.RemoveMonitorObjectReferences();

			var originalStartEpochIsAfter = filter.StartEpochIsAfter;
			var originalStartEpochIsBefore = filter.StartEpochIsBefore;
			var utcNow = DateTime.UtcNow;
			if (filter.StartEpochIsAfter == null)
			{
				filter.StartEpochIsAfter = utcNow.AddYears(-1).SecondsSinceTheEpoch();
			}
			if (filter.StartEpochIsBefore == null)
			{
				filter.StartEpochIsBefore = utcNow.SecondsSinceTheEpoch();
			}
			var allAlerts = new ConcurrentBag<Alert>();

			var alertFilterList = ((long)filter.StartEpochIsAfter).ToDateTimeUtc()
				.GetChunkedTimeRangeList(((long)filter.StartEpochIsBefore).ToDateTimeUtc(), chunkSize)
				.Select(t =>
				{
					var newAlertFilter = filter.Clone();
					newAlertFilter.ResetSearch();
					newAlertFilter.StartEpochIsAfter = t.Item1.SecondsSinceTheEpoch() - 1; // Take one off to include anything raised on that exact second
					newAlertFilter.StartEpochIsBefore = t.Item2.SecondsSinceTheEpoch();
					return newAlertFilter;
				});
			await Task.WhenAll(alertFilterList.Select(async individualAlertFilter =>
			{
				await Task.Delay(randomGenerator.Next(0, 2000), default).ConfigureAwait(false);
				foreach (var alert in (await GetDeviceAlertsByIdNormalAsync(deviceId, individualAlertFilter, true).ConfigureAwait(false)).alerts)
				{
					allAlerts.Add(alert);
				}
			})).ConfigureAwait(false);

			filter.StartEpochIsAfter = originalStartEpochIsAfter;
			filter.StartEpochIsBefore = originalStartEpochIsBefore;

			return allAlerts.DistinctBy(a => a.Id).Take(filter.Take ?? int.MaxValue).ToList();
		}

		/// <summary>
		///		Get device alerts with an alert filter (not all properties in the filter are used)
		/// </summary>
		/// <param name="deviceId">The Device ID</param>
		/// <param name="filter">An AlertFilter</param>
		/// <param name="calledFromChunked"></param>
		/// <param name="cancellationToken">The CancellationToken</param>
		/// <returns>A List of Alerts and whether the limit was reached</returns>
		internal async Task<(List<Alert> alerts, bool limitReached)> GetDeviceAlertsByIdNormalAsync(
			int deviceId,
			AlertFilter filter,
			bool calledFromChunked = false,
			CancellationToken cancellationToken = default)
		{
			// Ensure that the filter CANNOT override the device ID set!
			filter.RemoveMonitorObjectReferences();

			// Ensure skip is set (or 0)
			if (filter.Skip == null)
			{
				filter.Skip = 0;
			}

			var maxAlertCount = int.MaxValue;
			if (filter.Take != null)
			{
				if (filter.Take > AlertsMaxTake)
				{
					maxAlertCount = (int)filter.Take;
					filter.Take = AlertsMaxTake;
				}
				else
				{
					maxAlertCount = (int)filter.Take;
				}
			}
			else
			{
				filter.Take = AlertsMaxTake;
			}

			var allAlerts = new List<Alert>();
			do
			{
				var page = await GetBySubUrlAsync<Page<Alert>>($"device/devices/{deviceId}/alerts?{filter.GetFilter()}", cancellationToken).ConfigureAwait(false);

				allAlerts.AddRange(page.Items.Where(alert => !allAlerts.Select(aa => aa.Id).Contains(alert.Id)).ToList());

				if (!calledFromChunked && allAlerts.Count >= 5000)
				{
					// When there are more than 5000 (anywhere near the 10,000 limit), return and use the chunked method instead
					return (new List<Alert>(), true);
				}

				if (page.Items?.Count == 0)
				{
					break;
				}

				if (filter.SearchId == null && !string.IsNullOrWhiteSpace(page.SearchId))
				{
					// We can re-use the searchId
					filter.SearchId = page.SearchId;
				}

				filter.Skip += AlertsMaxTake;
				filter.Take = Math.Min(AlertsMaxTake, maxAlertCount - allAlerts.Count);
			}
			while (filter.Take != 0);

			return (allAlerts, false);
		}
	}
}