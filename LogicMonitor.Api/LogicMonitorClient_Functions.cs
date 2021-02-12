using LogicMonitor.Api.Functions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	public partial class LogicMonitorClient
	{
		/// <summary>
		///     Gets all Devices that match the AppliesTo string.
		/// </summary>
		/// <param name="query">The AppliesTo query.  For more details, see: https://www.logicmonitor.com/support/settings/logicmodules/changing-what-a-datasource-applies-to/</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>A list of AppliesToMatches.  Each contains the Device Name and Id.  For more information about this Device, use GetAsync&lt;Device&gt;(deviceId)</returns>
		public async Task<List<AppliesToMatch>> GetAppliesToAsync(string query, CancellationToken cancellationToken = default)
		{
			var appliesToRequest = new AppliesToRequest
			{
				OriginalAppliesTo = "",
				CurrentAppliesTo = query
			};
			return (await PostAsync<AppliesToRequest, AppliesToResponse>(appliesToRequest, "functions", cancellationToken).ConfigureAwait(false)).CurrentMatches;
		}
	}
}