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
	}
}