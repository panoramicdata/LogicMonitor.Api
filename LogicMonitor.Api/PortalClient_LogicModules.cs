using LogicMonitor.Api.LogicModules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{   /// <summary>
	///     Metadata
	/// </summary>
	public partial class PortalClient
	{
		/// <summary>
		///     Gets LogicModule metadata
		/// </summary>
		/// <param name="logicModuleType">The LogicModule type.  Note that not all types are supported by LogicMonitor.</param>
		/// <param name="id">The LogicModule Id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<LogicModuleMetadata> GetLogicModuleMetadata(
			LogicModuleType logicModuleType,
			int id,
			CancellationToken cancellationToken = default)
		{
			return GetBySubUrlAsync<LogicModuleMetadata>(
						   $"setting/registry/metadata/{GetText()}/{id}",
						   cancellationToken);

			string GetText()
			{
				switch (logicModuleType)
				{
					case LogicModuleType.PropertySource:
						return "property_rule";

					case LogicModuleType.JobMonitor:
					case LogicModuleType.AppliesToFunction:
						throw new NotSupportedException($"LogicMonitor does not support metadata for {logicModuleType}s.");
					default:
						return logicModuleType.ToString().ToLowerInvariant();
				}
			}
		}

		/// <summary>
		/// Get a list of LogicModule updates
		/// </summary>
		/// <param name="logicModuleType"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>A LogicModuleUpdate collection</returns>
		public async Task<LogicModuleUpdateCollection> GetLogicModuleUpdates(
			LogicModuleType logicModuleType,
			CancellationToken cancellationToken = default)
		{
			var typeParameterValue = string.Empty;
			switch (logicModuleType)
			{
				case LogicModuleType.Unknown:
					throw new LogicMonitorApiException($"Unable to ask for LogicModules of type '{logicModuleType.ToString()}'.");
				case LogicModuleType.DataSource:
				case LogicModuleType.EventSource:
				case LogicModuleType.ConfigSource:
					typeParameterValue = logicModuleType.ToString().ToLower();
					break;
				case LogicModuleType.PropertySource:
					typeParameterValue = "propertyrules";
					break;
				case LogicModuleType.JobMonitor:
					typeParameterValue = "batchjob";
					break;
				case LogicModuleType.AppliesToFunction:
					typeParameterValue = "function";
					break;
				case LogicModuleType.SnmpSysOIDMap:
					typeParameterValue = "oid";
					break;
				case LogicModuleType.TopologySource:
					typeParameterValue = $"{logicModuleType.ToString().ToLower()}s";
					break;
			}

			return await PostAsync<LogicModuleUpdateCredential, LogicModuleUpdateCollection>(
				new LogicModuleUpdateCredential
				{
					CoreServer = "v129.core.logicmonitor.com",
					Username = "anonymouse",
					Password = "logicmonitor"
				},
				$"setting/logicmodules/listcore?type={typeParameterValue}",
				cancellationToken)
			.ConfigureAwait(false);
		}

		/// <summary>
		/// Mark a DataSource (from the repository) as audited. Find the version via GetLogicModuleUpdates
		/// </summary>
		/// <param name="dataSourceId"></param>
		/// <param name="auditVersion"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<DataSource> AuditDataSource(int dataSourceId, long auditVersion, CancellationToken cancellationToken = default)
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
		/// <returns></returns>
		public async Task<EventSource> AuditEventSource(int eventSourceId, long auditVersion, CancellationToken cancellationToken = default)
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
		/// <returns></returns>
		public async Task<ConfigSource> AuditConfigSource(int configSourceId, long auditVersion, CancellationToken cancellationToken = default)
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
		/// <returns></returns>
		public async Task<PropertySource> AuditPropertySource(int propertySourceId, long auditVersion, CancellationToken cancellationToken = default)
			=> await PostAsync<LogicModuleUpdateVersion, PropertySource>
			(
				new LogicModuleUpdateVersion { Version = auditVersion },
				$"setting/propertyrules/{propertySourceId}/audit", cancellationToken
			)
			.ConfigureAwait(false);

		///// <summary>
		///// CURRENTLY NOT SUPPORTED IN LM: Mark a TopologySource (from the repository) as audited. Find the version via GetLogicModuleUpdates
		///// </summary>
		///// <param name="topologySourceId"></param>
		///// <param name="auditVersion"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//public async Task<TopologySource> AuditTopologySource(int topologySourceId, long auditVersion, CancellationToken cancellationToken = default)
		//	=> await PostAsync<LogicModuleUpdateVersion, TopologySource>
		//	(
		//		new LogicModuleUpdateVersion { Version = auditVersion },
		//		$"setting/topologysources/{topologySourceId}/audit", cancellationToken
		//	)
		//	.ConfigureAwait(false);

		///// <summary>
		///// CURRENTLY NOT SUPPORTED IN LM: Mark a Job Monitor (from the repository) as audited. Find the version via GetLogicModuleUpdates
		///// </summary>
		///// <param name="jobMonitorId"></param>
		///// <param name="auditVersion"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//public async Task<JobMonitor> AuditJobMonitor(int jobMonitorId, long auditVersion, CancellationToken cancellationToken = default)
		//	=> await PostAsync<LogicModuleUpdateVersion, JobMonitor>
		//	(
		//		new LogicModuleUpdateVersion { Version = auditVersion },
		//		$"setting/batchjob/{jobMonitorId}/audit", cancellationToken
		//	)
		//	.ConfigureAwait(false);

		///// <summary>
		///// CURRENTLY NOT SUPPORTED IN LM: Mark an AppliesTo Function (from the repository) as audited. Find the version via GetLogicModuleUpdates
		///// </summary>
		///// <param name="appliesToFunctionId"></param>
		///// <param name="auditVersion"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//public async Task<AppliesToFunction> AuditAppliesToFunction(int appliesToFunctionId, long auditVersion, CancellationToken cancellationToken = default)
		//	=> await PostAsync<LogicModuleUpdateVersion, AppliesToFunction>
		//	(
		//		new LogicModuleUpdateVersion { Version = auditVersion },
		//		$"setting/function/{appliesToFunctionId}/audit", cancellationToken
		//	)
		//	.ConfigureAwait(false);

		/// <summary>
		/// Mark an SNMP SysOID Map (from the repository) as audited. Find the version via GetLogicModuleUpdates
		/// </summary>
		/// <param name="snmpSysOidMapId"></param>
		/// <param name="auditVersion"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<SnmpSysOidMap> AuditSnmpSysOidMap(int snmpSysOidMapId, long auditVersion, CancellationToken cancellationToken = default)
			=> await PostAsync<LogicModuleUpdateVersion, SnmpSysOidMap>
			(
				new LogicModuleUpdateVersion { Version = auditVersion },
				$"setting/oid/{snmpSysOidMapId}/audit", cancellationToken
			)
			.ConfigureAwait(false);
	}
}