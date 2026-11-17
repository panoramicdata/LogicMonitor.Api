namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	private const int AlertsMaxTake = 300;

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
			return [.. alerts.Where(a => a.IsCleared)];
		}

		return alertFilter.IsCleared == false ? [.. alerts.Where(a => !a.IsCleared)] : alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="alertFilter"></param>
	/// <param name="chunkSize"></param>
	public Task<List<Alert>> GetRestAlertsWithV84BugAsync(AlertFilter alertFilter, TimeSpan chunkSize)
		=> GetAlertsChunkedAsync(alertFilter, chunkSize, f => GetRestAlertsWithoutV84BugAsync(f, true, default));

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
		correctedAlertFilter.MonitorObjectId = null;
		var alertFilterMonitorObjectId = alertFilter?.MonitorObjectId;
		if (alertFilterMonitorObjectId.HasValue)
		{
			correctedAlertFilter.MonitorObjectName = await GetMonitorObjectNameAsync(alertFilter, alertFilterMonitorObjectId.Value, cancellationToken).ConfigureAwait(false);
		}

		return await GetRestAlertPagesAsync(correctedAlertFilter, calledFromChunked, cancellationToken).ConfigureAwait(false);
	}

	private async Task<string> GetMonitorObjectNameAsync(AlertFilter? alertFilter, int monitorObjectId, CancellationToken cancellationToken)
	{
		var alertTypesAreOnlyWebsite = alertFilter?.GetAlertTypes()?.All(alertType => alertType == AlertType.Website) ?? false;
		return alertTypesAreOnlyWebsite
			? (await GetAsync<Website>(monitorObjectId, cancellationToken).ConfigureAwait(false)).Name
			: (await GetAsync<Resource>(monitorObjectId, cancellationToken).ConfigureAwait(false)).DisplayName;
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
	public async Task<MessageTemplateSet> GetMessageTemplatesAsync(CancellationToken cancellationToken) => new()
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

	/// <summary>
	/// Add a note to a single alert
	/// </summary>
	/// <param name="alertId">The non-unique alert id</param>
	/// <param name="note">The note</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task AddAlertNoteAsync(
		string alertId,
		string note,
		CancellationToken cancellationToken)
		=> PostAsync<AlertAcknowledgement, EmptyResponse>(
			new AlertAcknowledgement { AcknowledgementComment = note },
			$"alert/alerts/{alertId}/note",
			cancellationToken);

	/// <summary>
	/// Escalate an alert to the next stage of its escalation chain
	/// </summary>
	/// <param name="alertId">The non-unique alert id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task EscalateAlertAsync(
		string alertId,
		CancellationToken cancellationToken)
		=> PostAsync<EmptyRequest, EmptyResponse>(
			new EmptyRequest(),
			$"alert/alerts/{alertId}/escalate",
			cancellationToken);
}
