namespace LogicMonitor.Api;

/// <summary>
///    Websites Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Gets a website group by full path.
	///    Returns null if not found.
	/// </summary>
	/// <param name="websiteGroupFullPath">The website group full path.  The root is "/".</param>
	/// <param name="cancellationToken">Optional cancellation token</param>
	public async Task<WebsiteGroup?> GetWebsiteGroupByFullPathAsync(string websiteGroupFullPath, CancellationToken cancellationToken)
	{
		if (websiteGroupFullPath is null)
		{
			throw new ArgumentNullException(nameof(websiteGroupFullPath));
		}

		if (string.IsNullOrWhiteSpace(websiteGroupFullPath) || websiteGroupFullPath == "/")
		{
			return await GetAsync<WebsiteGroup>(1, cancellationToken).ConfigureAwait(false);
		}
		// A non-root path has been specified.

		var fullpathsplit = websiteGroupFullPath.Split('/');
		var currentWebsiteGroupId = 1;
		foreach (var localPath in fullpathsplit)
		{
			var websiteGroupById = await GetAsync<WebsiteGroup>(currentWebsiteGroupId, cancellationToken).ConfigureAwait(false);
			var websiteOverview = websiteGroupById.ChildWebsiteGroups?.SingleOrDefault(sov => sov.Name == localPath);
			if (websiteOverview is null)
			{
				return null;
			}

			currentWebsiteGroupId = websiteOverview.Id;
		}

		return await GetBySubUrlAsync<WebsiteGroup>($"website/groups/{currentWebsiteGroupId}", cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///     Set single website property
	/// </summary>
	/// <param name="websiteId">The Website Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetWebsiteCustomPropertyAsync(
		int websiteId,
		string name,
		string? value,
		SetPropertyMode mode = SetPropertyMode.Automatic,
		CancellationToken cancellationToken = default)
	=>
		SetWebsiteOrWebsiteGroupPropertyAsync<Website>(websiteId, name, value, mode, cancellationToken);

	private async Task SetWebsiteOrWebsiteGroupPropertyAsync<T>(
		int id,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, IHasCustomProperties, new()
	{
		// Input checking
		switch (typeof(T).Name)
		{
			case nameof(WebsiteGroup):
			case nameof(Website):
				// OK
				break;
			default:
				throw new InvalidOperationException($"Only to be used for {nameof(Website)} and {nameof(WebsiteGroup)}.");
		}

		switch (mode)
		{
			case SetPropertyMode.Create:
			case SetPropertyMode.Update:
				if (value is null)
				{
					throw new InvalidOperationException("Value cannot be null if the mode is create or update.");
				}

				break;
			case SetPropertyMode.Delete:
				if (value is not null)
				{
					throw new ArgumentException("If mode is delete, value must be set to null", nameof(value));
				}

				break;
			default:
				break;
		}

		// NB it is not yet possible to do the following:
		//return SetCustomPropertyAsync(
		//		  websiteId,
		//		  name,
		//		  value,
		//		  mode,
		//		  "website/websites",
		//		  cancellationToken);

		var websiteOrGroup = await GetAsync<T>(id, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
		var existingCustomProperty = websiteOrGroup.CustomProperties.SingleOrDefault(cp => cp.Name == name);
		if (existingCustomProperty is not null)
		{
			switch (value)
			{
				case null:
					websiteOrGroup.CustomProperties.Remove(existingCustomProperty);
					break;
				default:
					existingCustomProperty.Value = value;
					break;
			}
		}
		else
		{
			if (mode == SetPropertyMode.Delete)
			{
				throw new LogicMonitorApiException("Can't delete a custom property that is not there.");
			}

			switch (value)
			{
				case null:
					break;
				default:
					websiteOrGroup.CustomProperties.Add(new EntityProperty
					{
						Name = name,
						Value = value
					});
					break;
			}
		}

		await PutAsync(websiteOrGroup, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///     Set single websiteGroup property
	/// </summary>
	/// <param name="websiteGroupId">The WebsiteGroup Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetWebsiteGroupCustomPropertyAsync(
		int websiteGroupId,
		string name,
		string? value,
		SetPropertyMode mode = SetPropertyMode.Automatic,
		CancellationToken cancellationToken = default)
	=>
		SetWebsiteOrWebsiteGroupPropertyAsync<WebsiteGroup>(websiteGroupId, name, value, mode, cancellationToken);

	/// <summary>
	/// get a list of websites for a group (Response may contain extra fields depending upon the type of check { PingCheck | WebCheck} being added)
	/// </summary>
	/// <param name="id">The parent website group id.  If not specified, the root id (1) is used.</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<Page<Website>> GetWebsitesByWebsiteGroupIdAsync(
		int id,
		Filter<Website> filter,
		CancellationToken cancellationToken)
		=> (await FilteredGetAsync($"website/groups/{id}/websites", filter, cancellationToken).ConfigureAwait(false));

	/// <summary>
	///     Get website properties, in the following order:
	///     - Custom
	///     - Auto
	///     - System
	///     - Inherit
	/// </summary>
	/// <param name="websiteId">The Website Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetWebsitePropertiesAsync(int websiteId, CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"website/websites/{websiteId}/properties", cancellationToken);

	/// <summary>
	///     Gets website Group properties
	/// </summary>
	/// <param name="websiteGroupId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetWebsiteGroupPropertiesAsync(int websiteGroupId, CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"website/groups/{websiteGroupId}/properties", cancellationToken);

	/// <summary>
	/// Get Alerts for a Website by ID
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter">The Alert Filter</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts</returns>
	public async Task<List<Alert>> GetWebsiteAlertsByIdAsync(
		int websiteId,
		AlertFilter filter,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the website ID set!
		filter.RemoveMonitorObjectReferences();

		var (alerts, limitReached) = await
			GetWebsiteAlertsByIdNormalAsync(websiteId, filter, false, cancellationToken)
			.ConfigureAwait(false);

		if (limitReached)
		{
			// Fall back to the chunked method
			alerts = await GetWebsiteAlertsByIdChunkedAsync(websiteId, filter, TimeSpan.FromHours(24)).ConfigureAwait(false);
		}

		if (filter?.IsCleared == true)
		{
			return alerts.Where(a => a.IsCleared).ToList();
		}

		if (filter?.IsCleared == false)
		{
			return alerts.Where(a => !a.IsCleared).ToList();
		}

		return alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter"></param>
	/// <param name="chunkSize">The chunk size (TimeSpan)</param>
	internal async Task<List<Alert>> GetWebsiteAlertsByIdChunkedAsync(
		int websiteId,
		AlertFilter filter,
		TimeSpan chunkSize)
	{
		// Ensure that the filter CANNOT override the website ID set!
		filter.RemoveMonitorObjectReferences();

		var originalStartEpochIsAfter = filter.StartEpochIsAfter;
		var originalStartEpochIsBefore = filter.StartEpochIsBefore;
		var utcNow = DateTime.UtcNow;
		filter.StartEpochIsAfter ??= utcNow.AddYears(-1).SecondsSinceTheEpoch();

		filter.StartEpochIsBefore ??= utcNow.SecondsSinceTheEpoch();

		var allAlerts = new ConcurrentBag<Alert>();

		var alertFilterList = ((long)filter.StartEpochIsAfter).ToDateTimeUtc()
			.GetChunkedTimeRangeList(((long)filter.StartEpochIsBefore).ToDateTimeUtc(), chunkSize)
			.Select(t =>
			{
				var newAlertFilter = filter.Clone();
				newAlertFilter.ResetSearch();
				newAlertFilter.StartEpochIsAfter = t.Item1.SecondsSinceTheEpoch() - 1; // Take one off to include anything raised on that exact second
				newAlertFilter.StartEpochIsBefore = t.Item2.SecondsSinceTheEpoch();
				return newAlertFilter;
			});
		await Task.WhenAll(alertFilterList.Select(async individualAlertFilter =>
		{
			await Task.Delay(_randomGenerator.Next(0, 2000), default).ConfigureAwait(false);
			foreach (var alert in (await GetWebsiteAlertsByIdNormalAsync(websiteId, individualAlertFilter, true, default).ConfigureAwait(false)).alerts)
			{
				allAlerts.Add(alert);
			}
		})).ConfigureAwait(false);

		filter.StartEpochIsAfter = originalStartEpochIsAfter;
		filter.StartEpochIsBefore = originalStartEpochIsBefore;

		return allAlerts.DistinctBy(a => a.Id).Take(filter.Take ?? int.MaxValue).ToList();
	}

	/// <summary>
	///		Get website alerts with an alert filter (not all properties in the filter are used)
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter">An AlertFilter</param>
	/// <param name="calledFromChunked"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts and whether the limit was reached</returns>
	internal async Task<(List<Alert> alerts, bool limitReached)> GetWebsiteAlertsByIdNormalAsync(
		int websiteId,
		AlertFilter filter,
		bool calledFromChunked = false,
		CancellationToken cancellationToken = default)
	{
		// Ensure that the filter CANNOT override the website ID set!
		filter.RemoveMonitorObjectReferences();

		// Ensure skip is set (or 0)
		filter.Skip ??= 0;

		var maxAlertCount = int.MaxValue;
		if (filter.Take is not null)
		{
			if (filter.Take > AlertsMaxTake)
			{
				maxAlertCount = (int)filter.Take;
				filter.Take = AlertsMaxTake;
			}
			else
			{
				maxAlertCount = (int)filter.Take;
			}
		}
		else
		{
			filter.Take = AlertsMaxTake;
		}

		var allAlerts = new List<Alert>();
		do
		{
			var page = await GetBySubUrlAsync<Page<Alert>>($"website/websites/{websiteId}/alerts?{filter.GetFilter()}", cancellationToken).ConfigureAwait(false);

			allAlerts.AddRange(page.Items.Where(alert => !allAlerts.Select(aa => aa.Id).Contains(alert.Id)).ToList());

			if (!calledFromChunked && allAlerts.Count >= 5000)
			{
				// When there are more than 5000 (anywhere near the 10,000 limit), return and use the chunked method instead
				return (new List<Alert>(), true);
			}

			if (page.Items?.Count == 0)
			{
				break;
			}

			if (filter.SearchId is null && !string.IsNullOrWhiteSpace(page.SearchId))
			{
				// We can re-use the searchId
				filter.SearchId = page.SearchId;
			}

			filter.Skip += AlertsMaxTake;
			filter.Take = Math.Min(AlertsMaxTake, maxAlertCount - allAlerts.Count);
		}
		while (filter.Take != 0);

		return (allAlerts, false);
	}

	/// <summary>
	/// Get a list of SDTs for a Website
	/// </summary>
	public Task<Page<ScheduledDownTime>> GetWebsiteSdtListAsync(
		int websiteId,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"website/websites/{websiteId}/sdts", filter, cancellationToken);
}
