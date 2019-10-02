using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Backup;
using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.Netscans;
using LogicMonitor.Api.OpsNotes;
using LogicMonitor.Api.ScheduledDownTimes;
using LogicMonitor.Api.Settings;
using LogicMonitor.Api.Users;
using LogicMonitor.Api.Websites;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	public partial class PortalClient
	{
		/// <summary>
		///     Backs up all supported aspects of LogicMonitor configuration to a file as a GZipped Json string.
		/// </summary>
		/// <param name="fileInfo"></param>
		/// <returns></returns>
		public async Task BackupAllToFileAsync(FileInfo fileInfo)
		{
			var _ = await BackupAsync(new ConfigurationBackupSpecification(true) { GzipFileInfo = fileInfo }).ConfigureAwait(false);
		}

		/// <summary>
		///     Gets a configuration backup.
		/// </summary>
		/// <param name="backupSpecification"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="progressFunc"></param>
		/// <returns>the configuration backup</returns>
		public async Task<ConfigurationBackup> BackupAsync(ConfigurationBackupSpecification backupSpecification, CancellationToken cancellationToken = default, Action<string> progressFunc = null)
		{
			var configurationBackup = new ConfigurationBackup();

			var progressReporter = ProgressReporter.StartNew(progressFunc);
			// AccountSettings
			if (backupSpecification.AccountSettings)
			{
				progressReporter.Notify("Account Settings\r\n");
				progressReporter.StartSubTask("- Account Settings");
				configurationBackup.AccountSettings = await GetAsync<AccountSettings>(cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Company Logo");
				configurationBackup.CompanyLogo = Convert.ToBase64String(await GetImageByteArrayAsync(ImageType.CompanyLogo, cancellationToken).ConfigureAwait(false));
				progressReporter.CompleteSubTaskAndStartNew("- Login Logo");
				configurationBackup.LoginLogo = Convert.ToBase64String(await GetImageByteArrayAsync(ImageType.LoginLogo, cancellationToken).ConfigureAwait(false));
				progressReporter.CompleteSubTaskAndStartNew("- Single Sign-on");
				configurationBackup.SingleSignOn = await GetAsync<SingleSignOn>(cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- New user message template");
				configurationBackup.NewUserMessageTemplate = await GetAsync<NewUserMessageTemplate>(cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Alerting
			if (backupSpecification.Alerting)
			{
				progressReporter.Notify("Alerting\r\n");
				progressReporter.StartSubTask("- Alert Rules");
				configurationBackup.AlertRules = await GetAllAsync<AlertRule>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Escalation Chains");
				configurationBackup.EscalationChains = await GetAllAsync<EscalationChain>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- External alert destinations");
				configurationBackup.ExternalAlertDestinations = await GetAllAsync<ExternalAlertDestination>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Message Template set");
				configurationBackup.MessageTemplateSet = await GetMessageTemplatesAsync(cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Recipient Groups");
				configurationBackup.RecipientGroups = await GetAllAsync<RecipientGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Collectors
			if (backupSpecification.Collectors)
			{
				progressReporter.Notify("Collectors\r\n");
				progressReporter.StartSubTask("- Collectors");
				configurationBackup.Collectors = await GetAllAsync<Collector>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Dashboards
			if (backupSpecification.Dashboards)
			{
				progressReporter.Notify("Dashboards\r\n");
				progressReporter.StartSubTask("- Dashboard Groups");
				// Get with associated widgets populated
				configurationBackup.DashboardGroups = await GetAllAsync<DashboardGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Dashboards");
				configurationBackup.Dashboards = await GetAllAsync<Dashboard>().ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Widgets");
				configurationBackup.Widgets = await GetAllAsync<Widget>().ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Devices
			if (backupSpecification.Devices)
			{
				progressReporter.Notify("Devices\r\n");
				progressReporter.StartSubTask("- Device Groups");
				configurationBackup.DeviceGroups = await GetAllAsync<DeviceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Devices");
				configurationBackup.Devices = await GetAllAsync<Device>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Integrations
			if (backupSpecification.Integrations)
			{
				progressReporter.Notify("Integrations\r\n");
				progressReporter.StartSubTask("- Integrations");
				configurationBackup.Integrations = await GetAllAsync<Integration>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// LogicModules

			// AppliesToFunctions
			if (backupSpecification.AppliesToFunctions)
			{
				progressReporter.Notify("AppliesToFunctions\r\n");
				progressReporter.StartSubTask("- AppliesToFunctions");
				configurationBackup.AppliesToFunctions = await GetAllAsync<AppliesToFunction>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// ConfigSources
			if (backupSpecification.ConfigSources)
			{
				progressReporter.Notify("ConfigSources\r\n");
				progressReporter.StartSubTask("- ConfigSources");
				configurationBackup.ConfigSources = await GetAllAsync<ConfigSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// DataSources
			if (backupSpecification.DataSources)
			{
				progressReporter.Notify("DataSources\r\n");
				progressReporter.StartSubTask("- DataSources");
				configurationBackup.DataSources = await GetAllAsync<DataSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- DataSource graphs");

				configurationBackup.DataSourceGraphs = new List<DataSourceGraph>();
				configurationBackup.DataSourceOverviewGraphs = new List<DataSourceGraph>();
				configurationBackup.DataSourceDataPoints = new List<DataPointConfiguration>();
				foreach (var dataSource in configurationBackup.DataSources)
				{
					var dataSourceGraphs = await GetDataSourceGraphsAsync(dataSource.Id, cancellationToken).ConfigureAwait(false);
					configurationBackup.DataSourceGraphs.AddRange(dataSourceGraphs);
					var dataSourceOverviewGraphs = (await GetDataSourceOverviewGraphsPageAsync(dataSource.Id, new Filter<DataSourceGraph> { Skip = 0, Take = 300 }, cancellationToken).ConfigureAwait(false)).Items;
					configurationBackup.DataSourceOverviewGraphs.AddRange(dataSourceOverviewGraphs);
				}
				progressReporter.StopSubTask();
			}

			// EventSources
			if (backupSpecification.EventSources)
			{
				progressReporter.Notify("EventSources\r\n");
				progressReporter.StartSubTask("- EventSources");

				configurationBackup.EventSources = await GetAllAsync<EventSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// JobMonitors
			if (backupSpecification.JobMonitors)
			{
				progressReporter.Notify("JobMonitors\r\n");
				progressReporter.StartSubTask("- JobMonitors");

				configurationBackup.JobMonitors = await GetAllAsync<JobMonitor>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// PropertySources
			if (backupSpecification.PropertySources)
			{
				progressReporter.Notify("PropertySources\r\n");
				progressReporter.StartSubTask("- PropertySources");

				configurationBackup.PropertySources = await GetAllAsync<PropertySource>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// SnmpSysOidMaps
			if (backupSpecification.SnmpSysOidMaps)
			{
				progressReporter.Notify("SnmpSysOidMaps\r\n");
				progressReporter.StartSubTask("- SnmpSysOidMaps");

				configurationBackup.SnmpSysOidMaps = await GetAllAsync<SnmpSysOidMap>(cancellationToken: cancellationToken).ConfigureAwait(false);

				progressReporter.StopSubTask();
			}

			// Logs
			if (backupSpecification.Logs)
			{
				progressReporter.Notify("Logs\r\n");
				progressReporter.StartSubTask("- Logs");
				configurationBackup.LogItems = await GetLogItemsAsync(null, cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Netscans
			if (backupSpecification.Netscans)
			{
				progressReporter.Notify("Netscans\r\n");
				progressReporter.StartSubTask("- Netscans");
				configurationBackup.Netscans = await GetAllAsync<Netscan>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// OpsNotes
			if (backupSpecification.OpsNotes)
			{
				progressReporter.Notify("Ops Notes\r\n");
				progressReporter.StartSubTask("- Ops Notes");
				configurationBackup.OpsNotes = await GetAllAsync<OpsNote>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// SDTs
			if (backupSpecification.ScheduledDownTimes)
			{
				progressReporter.Notify("Scheduled Down Times\r\n");
				progressReporter.StartSubTask("- Scheduled Down Times");
				configurationBackup.ScheduledDownTimes = await GetAllAsync<ScheduledDownTime>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Websites
			if (backupSpecification.Websites)
			{
				progressReporter.Notify("Websites\r\n");
				progressReporter.StartSubTask("- Website Groups");
				configurationBackup.WebsiteGroups = await GetAllAsync<WebsiteGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Websites");
				configurationBackup.Websites = await GetAllAsync<Website>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// Users
			if (backupSpecification.Users)
			{
				progressReporter.Notify("Users and Roles\r\n");
				progressReporter.StartSubTask("- RoleGroups");
				configurationBackup.RoleGroups = await GetAllAsync<RoleGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- UserGroups");
				configurationBackup.Roles = await GetAllAsync<Role>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- UserGroups");
				configurationBackup.UserGroups = await GetAllAsync<UserGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.CompleteSubTaskAndStartNew("- Users");
				configurationBackup.Users = await GetAllAsync<User>(cancellationToken: cancellationToken).ConfigureAwait(false);
				progressReporter.StopSubTask();
			}

			// This one must be done last, to ensure that all retrieved data is sent to file
			if (backupSpecification.GzipFileInfo != null)
			{
				progressReporter.Notify("Save\r\n");
				progressReporter.StartSubTask("- Writing to disk");
				SerializeAndCompress(configurationBackup, backupSpecification.GzipFileInfo.FullName);
				progressReporter.StopSubTask();
			}

			progressReporter.Stop();

			return configurationBackup;
		}

		private static byte[] SerializeAndCompress<T>(T objectToWrite, string filePath) where T : class
		{
			if (objectToWrite == null)
			{
				return null;
			}
			byte[] result = null;
			using (var outputStream = new FileStream(filePath, FileMode.Create))
			using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
			using (var sw = new StreamWriter(compressionStream))
			using (var jsonTextWriter = new JsonTextWriter(sw))
			{
				var serializer = JsonSerializer.Create(new JsonSerializerSettings
				{
					Converters = new List<JsonConverter>
						{
							new StringEnumConverter()
						},
					Formatting = Formatting.Indented
				});
				serializer.Serialize(jsonTextWriter, objectToWrite);
			}
			return result;
		}
	}
}