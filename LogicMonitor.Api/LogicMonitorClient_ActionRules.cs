namespace LogicMonitor.Api;

/// <summary>
///     Action chain and action rule portal interaction
/// </summary>
/// <remarks>Action chain and action rule CRUD uses the generic methods, for example GetAllAsync&lt;ActionRule&gt;</remarks>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Get whether an action rule is enabled
	/// </summary>
	/// <param name="actionRuleId">The action rule id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ActionRuleStatus> GetActionRuleStatusAsync(
		int actionRuleId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ActionRuleStatus>($"setting/action/rules/{actionRuleId}/status", cancellationToken);

	/// <summary>
	///     Enable or disable an action rule
	/// </summary>
	/// <param name="actionRuleId">The action rule id</param>
	/// <param name="enabled">Whether the action rule should be enabled</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetActionRuleStatusAsync(
		int actionRuleId,
		bool enabled,
		CancellationToken cancellationToken)
		=> PutAsync($"setting/action/rules/{actionRuleId}/status", new ActionRuleStatus { Enabled = enabled }, cancellationToken);
}
