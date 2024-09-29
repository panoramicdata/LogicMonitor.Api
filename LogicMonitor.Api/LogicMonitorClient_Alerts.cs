namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	private const int AlertsMaxTake = 300;
	private readonly Random _randomGenerator = new();

	/// <summary>
	///     Gets a list of alerts.
	///     If no alert filter is present, all alerts are returned, including inactive alerts
	/// </summary>
	/// <param name="alertFilter">An alert filter</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>a list of alerts that meet the filter</returns>
	public async Task<List<Alert>> GetAlertsAsync(AlertFilter alertFilter, CancellationToken cancellationToken)
	{
		var (alerts, limitReached) = await GetRestAlertsWithoutV84BugAsync(alertFilter, false, cancellationToken).ConfigureAwait(false);

		if (limitReached)
		{
			// Fall back to the chunked method
			alerts = await GetRestAlertsWithV84BugAsync(alertFilter, TimeSpan.FromHours(24)).ConfigureAwait(false);
		}

		if (alertFilter.IsCleared == true)
		{
			return alerts.Where(a => a.IsCleared).ToList();
		}

		if (alertFilter.IsCleared == false)
		{
			return alerts.Where(a => !a.IsCleared).ToList();
		}

		return alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="alertFilter"></param>
	/// <param name="chunkSize"></param>
	public async Task<List<Alert>> GetRestAlertsWithV84BugAsync(AlertFilter alertFilter, TimeSpan chunkSize)
	{
		var originalStartEpochIsAfter = alertFilter.StartEpochIsAfter;
		var originalStartEpochIsBefore = alertFilter.StartEpochIsBefore;
		var utcNow = DateTime.UtcNow;
		alertFilter.StartEpochIsAfter ??= utcNow.AddYears(-1).SecondsSinceTheEpoch();

		alertFilter.StartEpochIsBefore ??= utcNow.SecondsSinceTheEpoch();

		var allAlerts = new ConcurrentBag<Alert>();

		var alertFilterList = ((long)alertFilter.StartEpochIsAfter).ToDateTimeUtc()
			.GetChunkedTimeRangeList(((long)alertFilter.StartEpochIsBefore).ToDateTimeUtc(), chunkSize)
			.Select(t =>
			{
				var newAlertFilter = alertFilter.Clone();
				newAlertFilter.ResetSearch();
				newAlertFilter.StartEpochIsAfter = t.Item1.SecondsSinceTheEpoch() - 1; // Take one off to include anything raised on that exact second
				newAlertFilter.StartEpochIsBefore = t.Item2.SecondsSinceTheEpoch();
				return newAlertFilter;
			});
		await Task.WhenAll(alertFilterList.Select(async individualAlertFilter =>
		{
			await Task.Delay(_randomGenerator.Next(0, 2000), default).ConfigureAwait(false);
			foreach (var alert in (await GetRestAlertsWithoutV84BugAsync(individualAlertFilter, true, default).ConfigureAwait(false)).alerts)
			{
				allAlerts.Add(alert);
			}
		})).ConfigureAwait(false);

		alertFilter.StartEpochIsAfter = originalStartEpochIsAfter;
		alertFilter.StartEpochIsBefore = originalStartEpochIsBefore;

		return allAlerts.DistinctBy(a => a.Id).Take(alertFilter.Take ?? int.MaxValue).ToList();
	}

	internal async Task<(List<Alert> alerts, bool limitReached)> GetRestAlertsWithoutV84BugAsync(
		AlertFilter? alertFilter,
		bool calledFromChunked,
		CancellationToken cancellationToken)
	{
		var correctedAlertFilter = alertFilter?.Clone() ?? new AlertFilter
		{
			StartEpochIsAfter = 0
		};

		// There is a bug in LogicMonitor (https://jira.logicmonitor.com/browse/DEVTS-4968)
		// whereby the MonitorObjectId does not work in a filter.  If used, substitute the MonitorObjectName instead
		// If the MonitorObjectId is set, set the
		correctedAlertFilter.MonitorObjectId = null;
		var alertFilterMonitorObjectId = alertFilter?.MonitorObjectId;
		if (alertFilterMonitorObjectId.HasValue)
		{
			var alertTypesOrNull = alertFilter?.GetAlertTypes();
			var alertTypesAreOnlyWebsite = alertTypesOrNull?.All(alertType => alertType == AlertType.Website) ?? false;
			correctedAlertFilter.MonitorObjectName = alertTypesAreOnlyWebsite
				? (await GetAsync<Website>(alertFilterMonitorObjectId.Value, cancellationToken).ConfigureAwait(false)).Name
				: (await GetAsync<Resource>(alertFilterMonitorObjectId.Value, cancellationToken).ConfigureAwait(false)).DisplayName;
		}

		// If take is specified, do only that chunk.

		correctedAlertFilter.Skip ??= 0;

		int maxAlertCount;
		if (correctedAlertFilter.Take is not null)
		{
			if (correctedAlertFilter.Take > AlertsMaxTake)
			{
				maxAlertCount = (int)correctedAlertFilter.Take;
				correctedAlertFilter.Take = AlertsMaxTake;
			}
			else
			{
				maxAlertCount = (int)correctedAlertFilter.Take;
			}
		}
		else
		{
			maxAlertCount = int.MaxValue;
			correctedAlertFilter.Take = AlertsMaxTake;
		}

		var allAlerts = new List<Alert>();
		do
		{
			var page = await FilteredGetAsync("alert/alerts", correctedAlertFilter.GetFilter(), cancellationToken).ConfigureAwait(false);
			allAlerts.AddRange(page.Items.Where(alert => !allAlerts.Select(aa => aa.Id).Contains(alert.Id)).ToList());

			if (!calledFromChunked && allAlerts.Count >= 5000)
			{
				// LMREP-9012: when there are more than 5000 (anywhere near the 10,000 limit), return and use the chunked method instead
				return (new List<Alert>(), true);
			}

			// The page TotalCount is negative if there are more to come, but this is not particularly useful to us.
			// The only way to be sure (while bugs exist) is to keep fetching the next batch until no more results are returned.
			if (page.Items?.Count == 0)
			{
				break;
			}

			// Adjust the alertFilter
			if (correctedAlertFilter.SearchId is null && !string.IsNullOrWhiteSpace(page.SearchId))
			{
				// We can re-use the searchId
				correctedAlertFilter.SearchId = page.SearchId;
			}

			correctedAlertFilter.Skip += AlertsMaxTake;
			correctedAlertFilter.Take = Math.Min(AlertsMaxTake, maxAlertCount - allAlerts.Count);
		}
		while (correctedAlertFilter.Take != 0);
		return (allAlerts, false);
	}

	/// <summary>
	///     Gets a single alert.  The alertType is no longer required as the alert id now contains the type.
	/// </summary>
	/// <param name="id">Alert id</param>
	/// <param name="cancellationToken"></param>
	public Task<Alert> GetAlertAsync(string id, CancellationToken cancellationToken)
		 => GetAsync<Alert>(true, $"alert/alerts/{id}?needMessage=true", cancellationToken);

	/// <summary>
	/// Gets the history of a single alert.
	/// </summary>
	/// <param name="request">The parameters for the history request</param>
	/// <param name="cancellationToken">The token used to manage cancellation</param>
	/// <remarks>The number and duration of entries in the histogram varies by the duration of the reporting period</remarks>
	public Task<AlertHistory> GetAlertHistoryAsync(AlertHistoryRequest request, CancellationToken cancellationToken)
	{
		request.Validate();
		return GetBySubUrlAsync<AlertHistory>(request.SubUrl, cancellationToken);
	}

	/// <summary>
	/// Gets a message template set
	/// </summary>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <returns>The message template set</returns>
	public async Task<MessageTemplateSet> GetMessageTemplatesAsync(CancellationToken cancellationToken) => new MessageTemplateSet
	{
		AlertThrottledAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/alertThrottledAlert", cancellationToken).ConfigureAwait(false),
		AlertThrottledClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/alertThrottledAlert_clear", cancellationToken).ConfigureAwait(false),
		CollectorDownAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/agentDownAlert", cancellationToken).ConfigureAwait(false),
		CollectorDownClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/agentDownAlert_clear", cancellationToken).ConfigureAwait(false),
		CollectorFailoverAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/agentFailoverAlert", cancellationToken).ConfigureAwait(false),
		CollectorFailoverClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/agentFailoverAlert_clear", cancellationToken).ConfigureAwait(false),
		CollectorFailbackAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/agentFailBackAlert", cancellationToken).ConfigureAwait(false),
		DataSourceAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/alert", cancellationToken).ConfigureAwait(false),
		DataSourceClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/alert_clear", cancellationToken).ConfigureAwait(false),
		EventSourceAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/eventAlert", cancellationToken).ConfigureAwait(false),
		EventSourceClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/eventAlert_clear", cancellationToken).ConfigureAwait(false),
		JobMonitorAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/batchJobAlert", cancellationToken).ConfigureAwait(false),
		JobMonitorClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/batchJobAlert_clear", cancellationToken).ConfigureAwait(false),
		HostClusterAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/hostClusterAlert", cancellationToken).ConfigureAwait(false),
		HostClusterClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/hostClusterAlert_clear", cancellationToken).ConfigureAwait(false),
		WebsiteAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/websitealert", cancellationToken).ConfigureAwait(false),
		WebsiteClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/websitealert_clear", cancellationToken).ConfigureAwait(false),
		WebsitesOverallAlertMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/websiteoverallalert", cancellationToken).ConfigureAwait(false),
		WebsitesOverallClearMessageTemplate = await GetAsync<AlertMessageTemplate>(false, "setting/alert/alertTemplates/websiteoverallalert_clear", cancellationToken).ConfigureAwait(false),
	};

	/// <summary>
	/// Acknowledge an alert
	/// </summary>
	/// <param name="alertId">The non-unique alert id</param>
	/// <param name="acknowledgementComment">The acknowledgement comment</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task AcknowledgeAlertAsync(
		string alertId,
		string acknowledgementComment,
		CancellationToken cancellationToken)
		=> PostAsync<AlertAcknowledgement, EmptyResponse>(
			new AlertAcknowledgement { AcknowledgementComment = acknowledgementComment },
			$"alert/alerts/{alertId}/ack",
			cancellationToken
			);

	/// <summary>
	/// add alert notes
	/// </summary>
	/// <param name="alertIds">The non-unique alert ids</param>
	/// <param name="note">The note</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetAlertNoteAsync(
		IList<string> alertIds,
		string note,
		CancellationToken cancellationToken)
		=> PostAsync<AlertNote, EmptyResponse>(
			new AlertNote(alertIds, note),
			$"alert/alerts/note",
			cancellationToken);
}
