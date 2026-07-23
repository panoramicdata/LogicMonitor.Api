namespace LogicMonitor.Api;

/// <summary>
///     LM Logs data query (ingested log messages, distinct from audit/access <see cref="Logs.LogItem"/>s).
/// </summary>
public partial class LogicMonitorClient
{
	private static readonly TimeSpan LmLogsDefaultTimeout = TimeSpan.FromSeconds(30);
	private static readonly TimeSpan LmLogsPollInterval = TimeSpan.FromSeconds(1);

	/// <summary>
	///     Queries ingested LM Logs data, using the default 30 second completion timeout.
	/// </summary>
	/// <param name="request">The search parameters (query, time range, size).</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<LogQueryResult> GetLogsAsync(LogQueryRequest request, CancellationToken cancellationToken)
		=> GetLogsAsync(request, null, cancellationToken);

	/// <summary>
	///     Queries ingested LM Logs data.
	///     The LM Logs search API is asynchronous: this submits the search (POST log/search), then polls
	///     (POST log/search/{queryId}) until the matched lines are available, <c>progress</c> reaches 1.0,
	///     or the timeout elapses (in which case the last — possibly partial — response is returned).
	/// </summary>
	/// <param name="request">The search parameters (query, time range, size).</param>
	/// <param name="timeout">How long to poll for completion. Defaults to 30 seconds.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public async Task<LogQueryResult> GetLogsAsync(LogQueryRequest request, TimeSpan? timeout, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		// Step 1: submit the search.
		var submit = await PostAsync<LogQueryRequest, LogQueryResult>(request, "log/search", cancellationToken)
			.ConfigureAwait(false);
		if (string.IsNullOrEmpty(submit.QueryId))
		{
			throw new InvalidOperationException("LM Logs search submit did not return a queryId.");
		}

		// Step 2: poll until the search produces its logs / completes, or the timeout elapses.
		var deadline = DateTimeOffset.UtcNow + (timeout ?? LmLogsDefaultTimeout);
		while (true)
		{
			var result = await PostAsync<object, LogQueryResult>(new object(), $"log/search/{submit.QueryId}", cancellationToken)
				.ConfigureAwait(false);

			if (result.Logs is not null || result.Progress >= 1.0 || DateTimeOffset.UtcNow >= deadline)
			{
				return result;
			}

			await Task.Delay(LmLogsPollInterval, cancellationToken).ConfigureAwait(false);
		}
	}
}
