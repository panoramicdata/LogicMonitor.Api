namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	/// get netscan list
	/// </summary>
	/// <param name="cancellationToken"></param>
	public Task<Page<Netscan>> GetNetscanListAsync(
		CancellationToken cancellationToken)
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

	/// <summary>
	/// Add a new netscan
	/// </summary>
	public Task<Netscan> AddNetscanAsync(
		NetscanCreationDto body,
		CancellationToken cancellationToken)
		=> PostAsync<NetscanCreationDto, Netscan>(body, $"setting/netscans", cancellationToken);
}