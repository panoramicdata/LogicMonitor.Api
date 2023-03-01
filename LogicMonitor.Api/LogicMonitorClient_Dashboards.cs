namespace LogicMonitor.Api;

/// <summary>
///     Dashboard Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets all dashboards
	/// </summary>
	/// <param name="dashboardName"></param>
	/// <param name="cancellationToken"></param>
	[Obsolete("Use GetByNameAsync<Dashboard> instead", true)]
	public Task<Dashboard> GetDashboardByNameAsync(string dashboardName, CancellationToken cancellationToken)
		=> GetByNameAsync<Dashboard>(dashboardName, cancellationToken);

	/// <summary>
	///     Gets all dashboards
	/// </summary>
	/// <param name="dashboardName"></param>
	/// <param name="cancellationToken"></param>
	public async Task<List<Widget>?> GetWidgetsByDashboardNameAsync(string dashboardName, CancellationToken cancellationToken)
	{
		var dashboard = await GetByNameAsync<Dashboard>(dashboardName, cancellationToken).ConfigureAwait(false);
		if (dashboard is null)
		{
			return null;
		}

		return await GetWidgetsByDashboardIdAsync(dashboard.Id, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///     Gets dashboard group by full path
	/// </summary>
	/// <param name="dashboardGroupFullPath">The full path</param>
	/// <param name="cancellationToken"></param>
	public async Task<DashboardGroup> GetDashboardGroupByFullPathAsync(string dashboardGroupFullPath, CancellationToken cancellationToken)
		=> (await GetAllAsync(new Filter<DashboardGroup>
		{
			FilterItems = new List<FilterItem<DashboardGroup>>
			{
					new Eq<DashboardGroup>(nameof(DashboardGroup.FullPath), dashboardGroupFullPath)
			}
		}, cancellationToken).ConfigureAwait(false)).SingleOrDefault();

	/// <summary>
	///     Gets child dashboard groups given a dashboard group id
	/// </summary>
	/// <param name="parentDashboardGroupId">The Id of the parent dashboard group</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken"></param>
	public Task<List<DashboardGroup>> GetChildDashboardGroupsAsync(int parentDashboardGroupId, Filter<DashboardGroup>? filter = null, CancellationToken cancellationToken = default)
		=> GetAllAsync(filter, $"dashboard/groups/{parentDashboardGroupId}/groups", cancellationToken);

	/// <summary>
	///     Gets child dashboards given a dashboard group id
	/// </summary>
	/// <param name="parentDashboardGroupId">The Id of the parent dashboard group</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken"></param>
	public Task<List<Dashboard>> GetChildDashboardsAsync(int parentDashboardGroupId, Filter<Dashboard>? filter = null, CancellationToken cancellationToken = default)
		=> GetAllAsync(filter, $"dashboard/groups/{parentDashboardGroupId}/dashboards", cancellationToken);

	/// <summary>
	///     Gets widget data
	/// </summary>
	/// <param name="widgetId">The Id of the widget</param>
	/// <param name="start">The start date</param>
	/// <param name="end">The end date</param>
	/// <param name="cancellationToken"></param>
	public Task<WidgetData> GetWidgetDataAsync(
		int widgetId,
		DateTimeOffset start,
		DateTimeOffset end,
		CancellationToken cancellationToken)
	{
		if (start >= end)
		{
			throw new ArgumentException("The end should be after the start.", nameof(end));
		}
		// Start is before end
		if (end > DateTimeOffset.UtcNow)
		{
			throw new ArgumentException("The end should not be in the future.", nameof(end));
		}
		// Start and end are in the past

		return GetBySubUrlAsync<WidgetData>($"dashboard/widgets/{widgetId}/data?time=zoom&start={start.ToUnixTimeSeconds()}&end={end.ToUnixTimeSeconds()}", cancellationToken);
	}

	/// <summary>
	///     Gets widgets for a given dashboard
	/// </summary>
	/// <param name="dashboardId">The Id of the dashboard</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<Widget>> GetWidgetsByDashboardIdAsync(
		int dashboardId,
		CancellationToken cancellationToken)
		=> (await GetAsync(new Filter<Widget>
		{
			Take = 100,
			FilterItems = new List<FilterItem<Widget>>
			{
					new Eq<Widget>(nameof(Widget.DashboardId), dashboardId)
			}
		}
		, cancellationToken).ConfigureAwait(false)).Items;

	/// <summary>
	///     Saves a widget
	/// </summary>
	/// <param name="widget">The widget to save</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task SaveNewWidgetAsync(
		HtmlWidget widget,
		CancellationToken cancellationToken)
		=> await PostAsync<HtmlWidget, HtmlWidget>(widget, "dashboard/widgets", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Delete a dashboard group and all child dashboard groups and their dashboards
	/// </summary>
	/// <param name="id">The dashboard group id</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public async Task DeleteDashboardGroupRecursivelyByIdAsync(int id, CancellationToken cancellationToken)
	{
		var dashboardGroup = await GetAsync<DashboardGroup>(id, cancellationToken).ConfigureAwait(false);

		// Delete its dashboards
		foreach (var dashboard in dashboardGroup.Dashboards)
		{
			await DeleteAsync(dashboard, cancellationToken: cancellationToken).ConfigureAwait(false);
		}

		// Delete its child dashboard groups
		var dashboardGroupPage = await GetAsync(new Filter<DashboardGroup>
		{
			FilterItems = new List<FilterItem<DashboardGroup>>
				{
					new Ne<DashboardGroup>(nameof(DashboardGroup.Id), 1),
					new Eq<DashboardGroup>(nameof(DashboardGroup.ParentId), id)
				}
		}, cancellationToken).ConfigureAwait(false);
		foreach (var childDashboardGroup in dashboardGroupPage.Items)
		{
			await DeleteDashboardGroupRecursivelyByIdAsync(childDashboardGroup.Id, cancellationToken).ConfigureAwait(false);
		}

		// Delete itself
		await DeleteAsync(dashboardGroup, cancellationToken: cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// update widget (Based upon widget type the request and response may contain additional attributes. Please refer models corresponding to specific widget type at the bottom of this page to check the attributes)
	/// </summary>
	/// <param name="id">The widget id</param>
	/// <param name="body">The body</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task PatchWidgetByIdAsync(
		int id,
		Widget body,
		CancellationToken cancellationToken)
		=> await PutAsync(
			$"dashboard/widgets/{id}",
			body,
			cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// get widget by id (Based upon widget type the response may contain additional attributes. Please refer models corresponding to specific widget type at the bottom of this page to check the attributes)
	/// </summary>
	/// <param name="id">The Widget ID</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Widget> GetWidgetByIdAsync(
		int id,
		CancellationToken cancellationToken)
		=> await GetAsync<Widget>(id, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// get widget list (Based upon widget type the response may contain additional attributes. Please refer models corresponding to specific widget type at the bottom of this page to check the attributes)
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <param name="fields"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="filter"></param>
	public async Task<Page<Widget>> GetWidgetListAsync(
		CancellationToken cancellationToken,
		string fields = null,
		int? size = 50,
		int? offset = 0,
		string filter = null)
		=> await GetBySubUrlAsync<Page<Widget>>($"/dashboard/widgets?fields={fields}&size={size}&offset={offset}&filter={filter}", cancellationToken);

	/// <summary>
	/// get widget data (Based upon widget type the response may contain additional attributes. Please refer models corresponding to specific widget type at the bottom of this page to check the attributes)
	/// </summary>
	/// <param name="id">Widget id</param>
	/// <param name="cancellationToken"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <param name="format"></param>
	public async Task<WidgetData> GetWidgetDataByIdAsync(
		int id,
		CancellationToken cancellationToken,
		int? start = null,
		int? end = null,
		string format = null
		)
		=> await GetBySubUrlAsync<WidgetData>($"dashboard/widgets/{id}/data?start={start}&end={end}&format={format}", cancellationToken);
}
