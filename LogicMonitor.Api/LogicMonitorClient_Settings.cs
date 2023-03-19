using Microsoft.Extensions.Primitives;

namespace LogicMonitor.Api;

/// <summary>
///     Settings Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the time zone setting for the current user
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The time zone setting</returns>
	public Task<TimeZoneSetting> GetTimeZoneSettingAsync(CancellationToken cancellationToken)
		=> GetAsync<TimeZoneSetting>(false, "setting/timezone", cancellationToken);

	/// <summary>
	///     Gets the roles for the current user
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Role>> GetRolesForCurrentUserPageAsync(Filter<Role> filter, CancellationToken cancellationToken)
	{
		if (filter is not null && filter.Order is null)
		{
			filter.Order = new Order<Role>
			{
				Property = nameof(Role.Name),
				Direction = OrderDirection.Asc
			};
		}

		return GetAsync<Page<Role>>(false, $"setting/roles?{filter}", cancellationToken);
	}

	/// <summary>
	///     Gets the escalation chains
	/// </summary>
	/// <param name="filter">The escalation chain filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The escalation chains</returns>
	public Task<Page<EscalationChain>> GetEscalationChainsPageAsync(Filter<EscalationChain> filter, CancellationToken cancellationToken)
	{
		if (filter is not null && filter.Order is null)
		{
			filter.Order = new Order<EscalationChain>
			{
				Property = nameof(EscalationChain.Name),
				Direction = OrderDirection.Asc
			};
		}

		return GetAsync<Page<EscalationChain>>(false, $"setting/alert/chains?{filter}", cancellationToken);
	}

	/// <summary>
	///     Saves an alert rule
	/// </summary>
	/// <param name="alertRule">The alert rule</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task SaveAlertRuleAsync(AlertRule alertRule, CancellationToken cancellationToken)
		=> await PutAsync($"setting/alert/rules/{alertRule.Id}?data=%5Bobject+Object%5D", alertRule, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// get user list
	/// </summary>
	public async Task<Page<Admin>> GetAdminListAsync(CancellationToken cancellationToken = default)
		=> await GetBySubUrlAsync<Page<Admin>>("$setting/admins", cancellationToken);

	/// <summary>
	/// get integration audit logs list
	/// </summary>
	/// <param name="cancellationToken"></param>
	public async Task<Page<IntegrationAuditLog>> GetIntegrationAuditLogsAsync(
		CancellationToken cancellationToken = default)
		=> await GetBySubUrlAsync<Page<IntegrationAuditLog>>("$setting/integrations/auditlogs", cancellationToken);

	/// <summary>
	/// get alert rule by id
	/// </summary>
	/// <param name="id">The alert rule id</param>
	/// <param name="fields"></param>
	/// <param name="cancellationToken"></param>
	public async Task<AlertRule> GetAlertRuleAsync(
		int id,
		string? fields,
		CancellationToken cancellationToken = default)
		=> await GetBySubUrlAsync<AlertRule>($"setting/alert/rules/{id}?fields={fields}", cancellationToken);

	/// <summary>
	/// delete alert rule
	/// </summary>
	/// <param name="id">The alert rule id</param>
	/// <param name="cancellationToken"></param>
	public async Task DeleteAlertRuleAsync(
		int id,
		CancellationToken cancellationToken)
		=> await DeleteAsync($"setting/alert/rules/{id}", cancellationToken);

	/// <summary>
	/// add alert rule
	/// </summary>
	/// <param name="body">The rule to be added</param>
	/// <param name="cancellationToken"></param>
	public async Task<AlertRule> AddAlertRuleAsync(
		AlertRule body,
		CancellationToken cancellationToken)
		=> await PostAsync<AlertRule, AlertRule>(body, $"setting/alert/rules", cancellationToken);

	/// <summary>
	/// get alert rule list
	/// </summary>
	public async Task<Page<AlertRule>> GetAlertRuleListAsync(
		Filter<AlertRule> filter,
		CancellationToken cancellationToken)
		=> await FilteredGetAsync<AlertRule>($"setting/alert/rules", filter, cancellationToken);
}
