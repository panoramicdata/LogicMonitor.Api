namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///    Lists netscan policy executions in a given time period
	/// </summary>
	/// <param name="netscanPolicyId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<NetscanExecutionResponse>> GetNetscanPolicyExecutionsAsync(int netscanPolicyId, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<NetscanExecutionResponse>>($"setting/netscans/policies/{netscanPolicyId}/executions/done", cancellationToken);

	/// <summary>
	/// get netscan list
	/// </summary>
	/// <param name="fields"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	public Task<NetscanPaginationResponse> GetNetscanList(
		string? fields,
		string? filter,
		CancellationToken cancellationToken,
		int size = 50,
		int offset = 0)
		=> GetBySubUrlAsync<NetscanPaginationResponse>($"setting/netscans", cancellationToken);
}