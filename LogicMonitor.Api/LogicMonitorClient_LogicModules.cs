namespace LogicMonitor.Api;

/// <summary>
///     Metadata
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets LogicModule metadata
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type.  Note that not all types are supported by LogicMonitor.</param>
	/// <param name="id">The LogicModule Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<LogicModuleMetadata> GetLogicModuleMetadata(
		LogicModuleType logicModuleType,
		int id,
		CancellationToken cancellationToken)
	{
		return GetBySubUrlAsync<LogicModuleMetadata>(
						$"setting/registry/metadata/{GetText()}/{id}",
						cancellationToken);

		string GetText()
		{
			return logicModuleType switch
			{
				LogicModuleType.PropertySource => "property_rule",
				LogicModuleType.JobMonitor or LogicModuleType.AppliesToFunction => throw new NotSupportedException($"LogicMonitor does not support metadata for {logicModuleType}s."),
				_ => logicModuleType.ToString().ToLowerInvariant(),
			};
		}
	}

	/// <summary>
	/// Get a list of LogicModule updates
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="repositoryVersion">The LogicMonitor repository version</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A LogicModuleUpdate collection</returns>
	public async Task<LogicModuleUpdateCollection> GetLogicModuleUpdates(
		LogicModuleType logicModuleType,
		int repositoryVersion,
		CancellationToken cancellationToken)
	{
		var typeParameter = string.Empty;
		switch (logicModuleType)
		{
			case LogicModuleType.All:
				// No parameter
				break;
			case LogicModuleType.DataSource:
			case LogicModuleType.EventSource:
			case LogicModuleType.ConfigSource:
				typeParameter = $"?type={logicModuleType.ToString().ToLower(CultureInfo.InvariantCulture)}";
				break;
			case LogicModuleType.PropertySource:
				typeParameter = "?type=propertyrules";
				break;
			case LogicModuleType.JobMonitor:
				typeParameter = "?type=batchjob";
				break;
			case LogicModuleType.AppliesToFunction:
				typeParameter = "?type=function";
				break;
			case LogicModuleType.SnmpSysOIDMap:
				typeParameter = "?type=oid";
				break;
			case LogicModuleType.TopologySource:
				typeParameter = $"?type={logicModuleType.ToString().ToLower(CultureInfo.InvariantCulture)}s";
				break;
		}

		return await PostAsync<LogicModuleUpdateCredential, LogicModuleUpdateCollection>(
			new LogicModuleUpdateCredential
			{
				CoreServer = GetLogicModuleRepositoryUrl(repositoryVersion)
			},
			$"setting/logicmodules/listcore{typeParameter}",
			cancellationToken)
		.ConfigureAwait(false);
	}

	/// <summary>
	/// Mark a DataSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public async Task<DataSource> AuditDataSource(int dataSourceId, long auditVersion, CancellationToken cancellationToken)
		=> await PostAsync<LogicModuleUpdateVersion, DataSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/datasources/{dataSourceId}/audit", cancellationToken
		)
		.ConfigureAwait(false);

	/// <summary>
	/// Mark an EventSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="eventSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public async Task<EventSource> AuditEventSource(int eventSourceId, long auditVersion, CancellationToken cancellationToken)
		=> await PostAsync<LogicModuleUpdateVersion, EventSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/eventsources/{eventSourceId}/audit", cancellationToken
		)
		.ConfigureAwait(false);

	/// <summary>
	/// Mark a ConfigSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="configSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public async Task<ConfigSource> AuditConfigSource(int configSourceId, long auditVersion, CancellationToken cancellationToken)
		=> await PostAsync<LogicModuleUpdateVersion, ConfigSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/configsources/{configSourceId}/audit", cancellationToken
		)
		.ConfigureAwait(false);

	/// <summary>
	/// Mark a PropertySource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="propertySourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public async Task<PropertySource> AuditPropertySource(int propertySourceId, long auditVersion, CancellationToken cancellationToken)
		=> await PostAsync<LogicModuleUpdateVersion, PropertySource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/propertyrules/{propertySourceId}/audit", cancellationToken
		)
		.ConfigureAwait(false);

	/// <summary>
	/// Import a LogicModule
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type (except SNMP SysOID Maps - for those use ImportSnmpSysOidMap)</param>
	/// <param name="logicModuleNames">A list of LogicModule names</param>
	/// <param name="repositoryVersion">The LogicMonitor repository version</param>
	/// <param name="cancellationToken"></param>
	public async Task ImportLogicModules(
		LogicModuleType logicModuleType,
		List<string> logicModuleNames,
		int repositoryVersion,
		CancellationToken cancellationToken)
	{
		var typeEndpoint = logicModuleType switch
		{
			LogicModuleType.DataSource or LogicModuleType.EventSource or LogicModuleType.ConfigSource or LogicModuleType.TopologySource => $"{logicModuleType.ToString().ToLower(CultureInfo.InvariantCulture)}s",
			LogicModuleType.PropertySource => "propertyrules",
			LogicModuleType.JobMonitor => "batchjobs",
			LogicModuleType.AppliesToFunction => "functions",
			LogicModuleType.SnmpSysOIDMap => throw new NotSupportedException($"Unsupported import type. Use {nameof(ImportSnmpSysOidMap)} instead."),
			_ => throw new NotSupportedException("Unsupported import type."),
		};
		await PostAsync<LogicModuleImportObject, object>
		(
			// OK for Datasources, EventSources, ConfigSources, PropertySources, TopologySources, AppliesToFunctions
			new LogicModuleImportObject
			{
				CoreServer = GetLogicModuleRepositoryUrl(repositoryVersion),
				ImportDataSources = logicModuleNames
			},
			$"setting/{typeEndpoint}/importcore",
			cancellationToken
		)
		.ConfigureAwait(false);
	}

	/// <summary>
	/// Import a LogicModule
	/// </summary>
	/// <param name="snmpSysOidMapImportItems">A list of LogicModule names</param>
	/// <param name="repositoryVersion">The LogicMonitor Repository version</param>
	/// <param name="cancellationToken"></param>
	public async Task ImportSnmpSysOidMap(
		List<SnmpSysOidMapImportItem> snmpSysOidMapImportItems,
		int repositoryVersion,
		CancellationToken cancellationToken)
	{
		await PostAsync<SnmpSysOidMapImportObject, object>
		(
			new SnmpSysOidMapImportObject
			{
				CoreServer = GetLogicModuleRepositoryUrl(repositoryVersion),
				ImportOids = snmpSysOidMapImportItems
			},
			"setting/oids/importcore",
			cancellationToken
		)
		.ConfigureAwait(false);
		return;
	}

	private static string GetLogicModuleRepositoryUrl(int repositoryVersion)
		=> repositoryVersion <= 0
		? throw new ArgumentOutOfRangeException(nameof(repositoryVersion), "LogicModule Repository Version expected to be greater than 0.")
		: $"v{repositoryVersion}.core.logicmonitor.com";
}
