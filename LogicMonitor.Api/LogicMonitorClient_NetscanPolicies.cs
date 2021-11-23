using LogicMonitor.Api.Netscans;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///    Lists netscan policy executions in a given time period
	/// </summary>
	/// <param name="netscanPolicyId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<NetscanExecutionResponse>> GetNetscanPolicyExecutionsAsync(int netscanPolicyId, CancellationToken cancellationToken = default)
		=> GetBySubUrlAsync<Page<NetscanExecutionResponse>>($"setting/netscans/policies/{netscanPolicyId}/executions/done", cancellationToken);
}
