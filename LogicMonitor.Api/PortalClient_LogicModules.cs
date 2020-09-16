using LogicMonitor.Api.LogicModules;
using System;
using System.Collections.Generic;
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
		/// <param name="logicModuleType">The LogicModule type</param>
		/// <param name="versionOverride">The LogicMonitor version override</param>
		/// <param name="cancellationToken"></param>
		/// <returns>A LogicModuleUpdate collection</returns>
		public async Task<LogicModuleUpdateCollection> GetLogicModuleUpdates(
			LogicModuleType logicModuleType,
			int? versionOverride = null,
			CancellationToken cancellationToken = default)
		{
			versionOverride ??= 141; // TODO: use the actual version we want
			var typeParameter = string.Empty;
			switch (logicModuleType)
			{
				case LogicModuleType.All:
					// No parameter
					break;
				case LogicModuleType.DataSource:
				case LogicModuleType.EventSource:
				case LogicModuleType.ConfigSource:
					typeParameter = $"?type={logicModuleType.ToString().ToLower()}";
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
					typeParameter = $"?type={logicModuleType.ToString().ToLower()}s";
					break;
			}

			return await PostAsync<LogicModuleUpdateCredential, LogicModuleUpdateCollection>(
				new LogicModuleUpdateCredential
				{
					CoreServer = $"v{versionOverride}.core.logicmonitor.com"
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

		/// <summary>
		/// Import a LogicModule
		/// </summary>
		/// <param name="logicModuleType">The LogicModule type (except SNMP SysOID Maps - for those use ImportSnmpSysOidMap)</param>
		/// <param name="logicModuleNames">A list of LogicModule names</param>
		/// <param name="versionOverride">The LogicMonitor version override</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task ImportLogicModules(
			LogicModuleType logicModuleType,
			List<string> logicModuleNames,
			int? versionOverride = null,
			CancellationToken cancellationToken = default)
		{
			versionOverride ??= 141; // TODO: use the actual version we want

			string typeEndpoint;
			switch (logicModuleType)
			{
				case LogicModuleType.DataSource:
				case LogicModuleType.EventSource:
				case LogicModuleType.ConfigSource:
				case LogicModuleType.TopologySource:
					typeEndpoint = $"{logicModuleType.ToString().ToLower()}s";
					break;
				case LogicModuleType.PropertySource:
					typeEndpoint = "propertyrules";
					break;
				case LogicModuleType.JobMonitor:
					typeEndpoint = "batchjobs";
					break;
				case LogicModuleType.AppliesToFunction:
					typeEndpoint = "functions";
					break;
				case LogicModuleType.SnmpSysOIDMap:
					throw new NotSupportedException($"Unsupported import type. Use {nameof(ImportSnmpSysOidMap)} instead.");
				default:
					throw new NotSupportedException("Unsupported import type.");
			}

			await PostAsync<LogicModuleImportObject, object>
			(
				// OK for Datasources, EventSources, ConfigSources, PropertySources, TopologySources, AppliesToFunctions
				new LogicModuleImportObject
				{
					CoreServer = $"v{versionOverride}.core.logicmonitor.com",
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
		/// <param name="versionOverride">The LogicMonitor version override</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task ImportSnmpSysOidMap(
			List<SnmpSysOidMapImportItem> snmpSysOidMapImportItems,
			int? versionOverride = null,
			CancellationToken cancellationToken = default)
		{
			versionOverride ??= 141; // TODO: use the actual version we want

			await PostAsync<SnmpSysOidMapImportObject, object>
			(
				new SnmpSysOidMapImportObject
				{
					CoreServer = $"v{versionOverride}.core.logicmonitor.com",
					ImportOids = snmpSysOidMapImportItems
				},
				"setting/oids/importcore",
				cancellationToken
			)
			.ConfigureAwait(false);
			return;
		}
	}
}