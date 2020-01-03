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
	}
}