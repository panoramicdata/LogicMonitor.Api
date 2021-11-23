using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Settings;
using LogicMonitor.Api.Users;
using System.Threading;
using System.Threading.Tasks;

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
	public Task<TimeZoneSetting> GetTimeZoneSettingAsync(CancellationToken cancellationToken = default)
		=> GetAsync<TimeZoneSetting>(false, "setting/timezone", cancellationToken);

	/// <summary>
	///     Gets the roles for the current user
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Role>> GetRolesForCurrentUserPageAsync(Filter<Role> filter, CancellationToken cancellationToken = default)
	{
		if (filter != null && filter.Order == null)
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
	public Task<Page<EscalationChain>> GetEscalationChainsPageAsync(Filter<EscalationChain> filter, CancellationToken cancellationToken = default)
	{
		if (filter != null && filter.Order == null)
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
	public async Task SaveAlertRuleAsync(AlertRule alertRule, CancellationToken cancellationToken = default)
		=> await PutAsync($"setting/alert/rules/{alertRule.Id}?data=%5Bobject+Object%5D", alertRule, cancellationToken).ConfigureAwait(false);
}
