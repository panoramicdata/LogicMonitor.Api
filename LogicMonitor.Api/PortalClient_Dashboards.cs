using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///     Dashboard Portal interaction
	/// </summary>
	public partial class PortalClient
	{
		/// <summary>
		///     Gets all dashboards
		/// </summary>
		/// <param name="dashboardName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[Obsolete("Use GetByNameAsync<Dashboard> instead", true)]
		public Task<Dashboard> GetDashboardByNameAsync(string dashboardName, CancellationToken cancellationToken = default)
			=> GetByNameAsync<Dashboard>(dashboardName, cancellationToken);

		/// <summary>
		///     Gets all dashboards
		/// </summary>
		/// <param name="dashboardName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<List<Widget>> GetWidgetsByDashboardNameAsync(string dashboardName, CancellationToken cancellationToken = default)
		{
			var dashboard = await GetByNameAsync<Dashboard>(dashboardName, cancellationToken).ConfigureAwait(false);
			if (dashboard == null)
			{
				return null;
			}

			return await GetWidgetsByDashboardIdAsync(dashboard.Id).ConfigureAwait(false);
		}

		/// <summary>
		///     Gets dashboard group by full path
		/// </summary>
		/// <param name="dashboardGroupFullPath">The full path</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<DashboardGroup> GetDashboardGroupByFullPathAsync(string dashboardGroupFullPath, CancellationToken cancellationToken = default)
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
		/// <returns></returns>
		public Task<List<DashboardGroup>> GetChildDashboardGroupsAsync(int parentDashboardGroupId, Filter<DashboardGroup> filter = null, CancellationToken cancellationToken = default)
			=> GetAllAsync(filter, $"dashboard/groups/{parentDashboardGroupId}/groups", cancellationToken);

		/// <summary>
		///     Gets child dashboards given a dashboard group id
		/// </summary>
		/// <param name="parentDashboardGroupId">The Id of the parent dashboard group</param>
		/// <param name="filter">The filter</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task<List<Dashboard>> GetChildDashboardsAsync(int parentDashboardGroupId, Filter<Dashboard> filter = null, CancellationToken cancellationToken = default)
			=> GetAllAsync(filter, $"dashboard/groups/{parentDashboardGroupId}/dashboards", cancellationToken);

		/// <summary>
		///     Gets widget data
		/// </summary>
		/// <param name="widgetId">The Id of the widget</param>
		/// <param name="start">The start date</param>
		/// <param name="end">The end date</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task<WidgetData> GetWidgetDataAsync(
			int widgetId,
			DateTimeOffset start,
			DateTimeOffset end,
			CancellationToken cancellationToken = default)
		{
			if(start >= end)
			{
				throw new ArgumentException("The end should be after the start.", nameof(end));
			}
			// Start is before end
			if(end > DateTimeOffset.UtcNow)
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
		/// <returns></returns>
		public async Task<List<Widget>> GetWidgetsByDashboardIdAsync(
			int dashboardId,
			CancellationToken cancellationToken = default)
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
			CancellationToken cancellationToken = default)
			=> await PostAsync<HtmlWidget, HtmlWidget>(widget, "dashboard/widgets", cancellationToken).ConfigureAwait(false);

		/// <summary>
		///     Delete a dashboard group and all child dashboard groups and their dashboards
		/// </summary>
		/// <param name="id">The dashboard group id</param>
		/// <param name="cancellationToken">The optional cancellation token</param>
		public async Task DeleteDashboardGroupRecursivelyByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			var dashboardGroup = await GetAsync<DashboardGroup>(id).ConfigureAwait(false);

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
			await DeleteAsync(dashboardGroup).ConfigureAwait(false);
		}
	}
}