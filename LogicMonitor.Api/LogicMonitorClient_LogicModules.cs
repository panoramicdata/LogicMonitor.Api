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
	public Task<LogicModuleMetadata> GetLogicModuleMetadataAsync(
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
	/// Gets the LM Exchange metadata for all LogicModules of all types, annotated with installation and
	/// upgrade state. This replaces the retired <see cref="GetLogicModuleUpdatesAsync"/> (listcore) mechanism.
	/// A module has an update available when <see cref="ExchangeLogicModule.HasUpdateAvailable"/> is true.
	/// Filter by <see cref="ExchangeLogicModule.Type"/> client-side to restrict to a specific LogicModule type.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>All exchange LogicModules, across every type</returns>
	public async Task<List<ExchangeLogicModule>> GetExchangeLogicModulesAsync(CancellationToken cancellationToken)
	{
		// This endpoint returns a bare JSON array rather than the usual PortalResponse envelope, and the
		// fields vary by module type and installation state. Fetch the raw body via XmlResponse (which
		// bypasses the envelope parsing) and project each element leniently onto ExchangeLogicModule
		// (unmodelled, type-specific fields are ignored).
		var response = await GetBySubUrlAsync<XmlResponse>("setting/logicmodules/metadata", cancellationToken)
			.ConfigureAwait(false);

		var serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
		{
			MissingMemberHandling = MissingMemberHandling.Ignore
		});

		return [.. JArray.Parse(response.Content).Select(token => token.ToObject<ExchangeLogicModule>(serializer)!)];
	}

	/// <summary>
	/// Get a list of LogicModule updates.
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A LogicModuleUpdate collection</returns>
	[Obsolete("LogicMonitor has retired the listcore endpoint this uses (it now requires an undocumented admin bearer token in the body and otherwise fails with 'bearerToken may not be null or empty'). Use GetExchangeLogicModulesAsync instead; a module has an update available when ExchangeLogicModule.HasUpdateAvailable is true.")]
	public async Task<LogicModuleUpdateCollection> GetLogicModuleUpdatesAsync(
		LogicModuleType logicModuleType,
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
			case LogicModuleType.DiagnosticSource:
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
			default:
				throw new NotSupportedException($"LogicModule type {logicModuleType} not supported.");
		}

		return await PostAsync<LogicModuleUpdateCredential, LogicModuleUpdateCollection>(
			new LogicModuleUpdateCredential
			{
				CoreServer = CoreServerName
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
	public Task<DataSource> AuditDataSourceAsync(int dataSourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, DataSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/datasources/{dataSourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark an EventSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="eventSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<EventSource> AuditEventSourceAsync(int eventSourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, EventSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/eventsources/{eventSourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a ConfigSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="configSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<ConfigSource> AuditConfigSourceAsync(int configSourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, ConfigSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/configsources/{configSourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a DiagnosticSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="diagnosticSourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<LogicMonitor.Api.LogicModules.DiagnosticSource> AuditDiagnosticSourceAsync(int diagnosticSourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, LogicMonitor.Api.LogicModules.DiagnosticSource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/diagnosticsources/{diagnosticSourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a PropertySource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="propertySourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<PropertySource> AuditPropertySourceAsync(int propertySourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, PropertySource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/propertyrules/{propertySourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a TopologySource (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="topologySourceId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<TopologySource> AuditTopologySourceAsync(int topologySourceId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, TopologySource>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/topologysources/{topologySourceId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a JobMonitor (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="jobMonitorId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<JobMonitor> AuditJobMonitorAsync(int jobMonitorId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, JobMonitor>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/batchjobs/{jobMonitorId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark an AppliesToFunction (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="appliesToFunctionId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<AppliesToFunction> AuditAppliesToFunctionAsync(int appliesToFunctionId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, AppliesToFunction>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/functions/{appliesToFunctionId}/audit", cancellationToken
		);

	/// <summary>
	/// Mark a SnmpSysOidMap (from the repository) as audited. Find the version via GetLogicModuleUpdates
	/// </summary>
	/// <param name="snmpSysOidMapId"></param>
	/// <param name="auditVersion"></param>
	/// <param name="cancellationToken"></param>
	public Task<SnmpSysOidMap> AuditSnmpSysOidMapAsync(int snmpSysOidMapId, long auditVersion, CancellationToken cancellationToken)
		=> PostAsync<LogicModuleUpdateVersion, SnmpSysOidMap>
		(
			new LogicModuleUpdateVersion { Version = auditVersion },
			$"setting/oids/{snmpSysOidMapId}/audit", cancellationToken
		);

	/// <summary>
	/// Import a LogicModule
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type (except SNMP SysOID Maps - for those use ImportSnmpSysOidMap)</param>
	/// <param name="logicModuleNames">A list of LogicModule names</param>
	/// <param name="cancellationToken"></param>
	public async Task ImportLogicModulesAsync(
		LogicModuleType logicModuleType,
		List<string> logicModuleNames,
		CancellationToken cancellationToken)
	{
		var typeEndpoint = logicModuleType switch
		{
			LogicModuleType.DataSource or LogicModuleType.EventSource or LogicModuleType.ConfigSource or LogicModuleType.TopologySource or LogicModuleType.DiagnosticSource => $"{logicModuleType.ToString().ToLower(CultureInfo.InvariantCulture)}s",
			LogicModuleType.PropertySource => "propertyrules",
			LogicModuleType.JobMonitor => "batchjobs",
			LogicModuleType.AppliesToFunction => "functions",
			LogicModuleType.SnmpSysOIDMap => throw new NotSupportedException($"Unsupported import type. Use {nameof(ImportSnmpSysOidMapAsync)} instead."),
			_ => throw new NotSupportedException("Unsupported import type."),
		};
		await PostAsync<LogicModuleImportObject, object>
		(
			// OK for Datasources, EventSources, ConfigSources, PropertySources, TopologySources, AppliesToFunctions
			new LogicModuleImportObject
			{
				CoreServer = CoreServerName,
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
	/// <param name="cancellationToken"></param>
	public async Task ImportSnmpSysOidMapAsync(
		List<SnmpSysOidMapImportItem> snmpSysOidMapImportItems,
		CancellationToken cancellationToken) =>
		await PostAsync<SnmpSysOidMapImportObject, object>
		(
			new SnmpSysOidMapImportObject
			{
				CoreServer = CoreServerName,
				ImportOids = snmpSysOidMapImportItems
			},
			"setting/oids/importcore",
			cancellationToken
		)
		.ConfigureAwait(false);

	private const string CoreServerName = "core.logicmonitor.com";
}
