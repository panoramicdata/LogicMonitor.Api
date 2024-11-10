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
	/// Add escalation chain by id
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	public Task<EscalationChain> AddEscalationChainAsync(
		EscalationChain body,
		CancellationToken cancellationToken)
		=> PostAsync<EscalationChain, EscalationChain>(body, $"setting/alert/chains", cancellationToken);

	/// <summary>
	/// Get escalation chain by id
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	public Task<EscalationChain> GetEscalationChainAsync(
		int id,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<EscalationChain>($"setting/alert/chains/{id}", cancellationToken);

	/// <summary>
	///     Saves an alert rule
	/// </summary>
	/// <param name="alertRule">The alert rule</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SaveAlertRuleAsync(AlertRule alertRule, CancellationToken cancellationToken)
		=> PutAsync($"setting/alert/rules/{alertRule.Id}?data=%5Bobject+Object%5D", alertRule, cancellationToken);

	/// <summary>
	/// get integration audit logs list
	/// </summary>
	public Task<Page<IntegrationAuditLog>> GetIntegrationAuditLogsAsync()
		=> GetIntegrationAuditLogsAsync(CancellationToken.None);

	/// <summary>
	/// get integration audit logs list
	/// </summary>
	/// <param name="cancellationToken"></param>
	public Task<Page<IntegrationAuditLog>> GetIntegrationAuditLogsAsync(
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<IntegrationAuditLog>>($"setting/integrations/auditlogs", cancellationToken);

	/// <summary>
	/// Add API tokens for a user
	/// </summary>
	/// <param name="adminId">The admin id of the user</param>
	/// <param name="body"></param>
	/// <param name="type"></param>
	/// <param name="cancellationToken"></param>
	public async Task<ApiToken> AddApiTokensAsync(
		int adminId,
		ApiToken body,
		string? type,
		CancellationToken cancellationToken)
		=> await PostAsync<ApiToken, ApiToken>(body, $"setting/admins/{adminId}/apitokens?type={type}", cancellationToken);

	/// <summary>
	/// Get a list of API tokens across users
	/// </summary>
	public Task<Page<ApiToken>> GetApiTokenListAsync(
		Filter<ApiToken> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ApiToken>>
		($"setting/admins/apitokens?{filter}",
			cancellationToken);

	/// <summary>
	/// Get external API stats info
	/// </summary>
	public async Task<Page<ExternalApiStats>> GetExternalApiAsync(
		CancellationToken cancellationToken)
		=> await GetBySubUrlAsync<Page<ExternalApiStats>>($"apiStats/externalApis", cancellationToken);


	/// <summary>
	/// Update applies to function
	/// </summary>
	public Task<AppliesToFunction> UpdateAppliesToFunctionAsync(
		AppliesToFunctionCreationDto body,
		int id,
		string reason,
		bool ignoreReference = false,
		CancellationToken cancellationToken = default)
		=> PostAsync<AppliesToFunctionCreationDto, AppliesToFunction>(
			body, $"setting/functions/{id}?reason={reason}&ignoreReference={ignoreReference}",
			cancellationToken);

	/// <summary>
	/// Get applies to function
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken">s</param>
	public Task<AppliesToFunction> GetAppliesToFunctionAsync(
		int id,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<AppliesToFunction>(
			$"setting/functions/{id}",
			cancellationToken);

	/// <summary>
	/// Add applies to function
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	public Task<AppliesToFunction> AddAppliesToFunctionAsync(
		AppliesToFunctionCreationDto body,
		CancellationToken cancellationToken)
		=> PostAsync<AppliesToFunctionCreationDto, AppliesToFunction>(
			body, $"setting/functions", cancellationToken);

	/// <summary>
	/// Get applies to function list
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<AppliesToFunction>> GetAppliesToFunctionListAsync(
		Filter<AppliesToFunction> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"setting/functions", filter, cancellationToken);

	/// <summary>
	/// Get metrics usage
	/// </summary>
	public Task<AccountSettings> GetMetricsAsync(
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<AccountSettings>($"metrics/usage", cancellationToken);
}
