namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Backs up all supported aspects of LogicMonitor configuration to a file as a GZipped Json string.
	/// </summary>
	/// <param name="fileInfo"></param>
	public async Task BackupAllToFileAsync(FileInfo fileInfo)
	{
		var _ = await BackupAsync(new ConfigurationBackupSpecification(true) { GzipFileInfo = fileInfo }, default)
			.ConfigureAwait(false);
	}

	/// <summary>
	///     Gets a configuration backup.
	/// </summary>
	/// <param name="backupSpecification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>the configuration backup</returns>
	public async Task<ConfigurationBackup> BackupAsync(ConfigurationBackupSpecification backupSpecification, CancellationToken cancellationToken)
	{
		if (backupSpecification is null)
		{
			throw new ArgumentNullException(nameof(backupSpecification));
		}

		var configurationBackup = new ConfigurationBackup();

		var progressReporter = ProgressReporter.StartNew(_logger);
		// AccountSettings
		if (backupSpecification.AccountSettings)
		{
			progressReporter.Notify("Account Settings");
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
			progressReporter.Notify("Alerting");
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
			progressReporter.Notify("Collectors");
			progressReporter.StartSubTask("- Collectors");
			configurationBackup.Collectors = await GetAllAsync<Collector>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Dashboards
		if (backupSpecification.Dashboards)
		{
			progressReporter.Notify("Dashboards");
			progressReporter.StartSubTask("- Dashboard Groups");
			// Get with associated widgets populated
			configurationBackup.DashboardGroups = await GetAllAsync<DashboardGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.CompleteSubTaskAndStartNew("- Dashboards");
			configurationBackup.Dashboards = await GetAllAsync<Dashboard>(cancellationToken).ConfigureAwait(false);
			progressReporter.CompleteSubTaskAndStartNew("- Widgets");
			configurationBackup.Widgets = await GetAllAsync<Widget>(cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Devices
		if (backupSpecification.Devices)
		{
			progressReporter.Notify("Devices");
			progressReporter.StartSubTask("- Device Groups");
			configurationBackup.DeviceGroups = await GetAllAsync<DeviceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.CompleteSubTaskAndStartNew("- Devices");
			configurationBackup.Devices = await GetAllAsync<Device>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Integrations
		if (backupSpecification.Integrations)
		{
			progressReporter.Notify("Integrations");
			progressReporter.StartSubTask("- Integrations");
			configurationBackup.Integrations = await GetAllAsync<Integration>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// LogicModules

		// AppliesToFunctions
		if (backupSpecification.AppliesToFunctions)
		{
			progressReporter.Notify("AppliesToFunctions");
			progressReporter.StartSubTask("- AppliesToFunctions");
			configurationBackup.AppliesToFunctions = await GetAllAsync<AppliesToFunction>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// ConfigSources
		if (backupSpecification.ConfigSources)
		{
			progressReporter.Notify("ConfigSources");
			progressReporter.StartSubTask("- ConfigSources");
			configurationBackup.ConfigSources = await GetAllAsync<ConfigSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// DataSources
		if (backupSpecification.DataSources)
		{
			progressReporter.Notify("DataSources");
			progressReporter.StartSubTask("- DataSources");
			configurationBackup.DataSources = await GetAllAsync<DataSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.CompleteSubTaskAndStartNew("- DataSource graphs");

			configurationBackup.DataSourceGraphs = new List<DataSourceOverviewGraph>();
			configurationBackup.DataSourceOverviewGraphs = new List<DataSourceOverviewGraph>();
			configurationBackup.DataSourceDataPoints = new List<DataPointConfiguration>();
			foreach (var dataSource in configurationBackup.DataSources)
			{
				var dataSourceGraphs = await GetDataSourceGraphsAsync(dataSource.Id, cancellationToken).ConfigureAwait(false);
				configurationBackup.DataSourceGraphs.AddRange(dataSourceGraphs);
				var dataSourceOverviewGraphs = (await GetDataSourceOverviewGraphsPageAsync(dataSource.Id, new Filter<DataSourceOverviewGraph> { Skip = 0, Take = 300 }, cancellationToken).ConfigureAwait(false)).Items;
				configurationBackup.DataSourceOverviewGraphs.AddRange(dataSourceOverviewGraphs);
			}

			progressReporter.StopSubTask();
		}

		// EventSources
		if (backupSpecification.EventSources)
		{
			progressReporter.Notify("EventSources");
			progressReporter.StartSubTask("- EventSources");

			configurationBackup.EventSources = await GetAllAsync<EventSource>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// JobMonitors
		if (backupSpecification.JobMonitors)
		{
			progressReporter.Notify("JobMonitors");
			progressReporter.StartSubTask("- JobMonitors");

			configurationBackup.JobMonitors = await GetAllAsync<JobMonitor>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// PropertySources
		if (backupSpecification.PropertySources)
		{
			progressReporter.Notify("PropertySources");
			progressReporter.StartSubTask("- PropertySources");

			configurationBackup.PropertySources = await GetAllAsync<PropertySource>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// SnmpSysOidMaps
		if (backupSpecification.SnmpSysOidMaps)
		{
			progressReporter.Notify("SnmpSysOidMaps");
			progressReporter.StartSubTask("- SnmpSysOidMaps");

			configurationBackup.SnmpSysOidMaps = await GetAllAsync<SnmpSysOidMap>(cancellationToken: cancellationToken).ConfigureAwait(false);

			progressReporter.StopSubTask();
		}

		// Logs
		if (backupSpecification.Logs)
		{
			progressReporter.Notify("Logs");
			progressReporter.StartSubTask("- Logs");
			configurationBackup.LogItems = await GetLogItemsAsync(null, cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Netscans
		if (backupSpecification.Netscans)
		{
			progressReporter.Notify("Netscans");
			progressReporter.StartSubTask("- Netscans");
			configurationBackup.Netscans = await GetAllAsync<Netscan>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// OpsNotes
		if (backupSpecification.OpsNotes)
		{
			progressReporter.Notify("Ops Notes");
			progressReporter.StartSubTask("- Ops Notes");
			configurationBackup.OpsNotes = await GetAllAsync<OpsNote>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// SDTs
		if (backupSpecification.ScheduledDownTimes)
		{
			progressReporter.Notify("Scheduled Down Times");
			progressReporter.StartSubTask("- Scheduled Down Times");
			configurationBackup.ScheduledDownTimes = await GetAllAsync<ScheduledDownTime>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Websites
		if (backupSpecification.Websites)
		{
			progressReporter.Notify("Websites");
			progressReporter.StartSubTask("- Website Groups");
			configurationBackup.WebsiteGroups = await GetAllAsync<WebsiteGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.CompleteSubTaskAndStartNew("- Websites");
			configurationBackup.Websites = await GetAllAsync<Website>(cancellationToken: cancellationToken).ConfigureAwait(false);
			progressReporter.StopSubTask();
		}

		// Users
		if (backupSpecification.Users)
		{
			progressReporter.Notify("Users and Roles");
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
		if (backupSpecification.GzipFileInfo is not null)
		{
			progressReporter.Notify("Save");
			progressReporter.StartSubTask("- Writing to disk");
			SerializeAndCompress(configurationBackup, backupSpecification.GzipFileInfo);
			progressReporter.StopSubTask();
		}

		progressReporter.Stop();

		return configurationBackup;
	}

	private static byte[] SerializeAndCompress<T>(T objectToWrite, FileInfo fileInfo) where T : class
	{
		if (objectToWrite is null)
		{
			return null;
		}

		byte[] result = null;
		using (var outputStream = new FileStream(fileInfo.FullName, FileMode.Create))
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

	/// <summary>
	/// Load a ConfigurationBackup from file
	/// </summary>
	/// <param name="fileInfo"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns></returns>
	public Task<ConfigurationBackup> LoadBackupAsync(FileInfo fileInfo, CancellationToken cancellationToken)
		=> LoadAsync<ConfigurationBackup>(fileInfo ?? throw new ArgumentNullException(nameof(fileInfo)), _logger, cancellationToken);

	private static async Task<T> LoadAsync<T>(
		FileInfo fileInfo,
		ILogger _logger,
		CancellationToken cancellationToken)
	{
		_logger.LogDebug($"{nameof(LoadBackupAsync)}: {{Message}}", "Loading file into memory");
		var bytes = File.ReadAllBytes(fileInfo.FullName);

		_logger.LogDebug($"{nameof(LoadBackupAsync)}: {{Message}}", "Decompressing");
		using var msi = new MemoryStream(bytes);
		using var mso = new MemoryStream();
		using var gs = new GZipStream(msi, CompressionMode.Decompress);
		await gs.CopyToAsync(mso).ConfigureAwait(false);
		var json = Encoding.UTF8.GetString(mso.ToArray());

		_logger.LogDebug($"{nameof(LoadBackupAsync)}: {{Message}}", "Deserializing");
		var myObject = JsonConvert.DeserializeObject<T>(json);

		_logger.LogDebug($"{nameof(LoadBackupAsync)}: {{Message}}", "Complete");
		return myObject ?? throw new FormatException("Unable to deserialize file");
	}
}
