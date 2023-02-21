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
	public Task<Page<Netscan>> GetNetscanListAsync(
		string? fields,
		string? filter,
		CancellationToken cancellationToken,
		int size = 50,
		int offset = 0)
		=> GetBySubUrlAsync<Page<Netscan>>($"setting/netscans", cancellationToken);

	/// <summary>
	/// get netscan by id
	/// </summary>
	/// <param name="id">The netscan id</param>
	/// <param name="cancellationToken"></param>
	public Task<Netscan> GetNetscanByIdAsync(
		int id,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Netscan>($"setting/netscans/{id}", cancellationToken);

}