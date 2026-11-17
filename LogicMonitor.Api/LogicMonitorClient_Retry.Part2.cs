namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{

	// Returns when the request should be retried; throws when it should not.
	private async Task WaitBeforeRetryOrThrowAsync(
		HttpMethod httpMethod,
		string subUrl,
		HttpResponseMessage httpResponseMessage,
		string prefix,
		RetryState state,
		bool retryServerErrors,
		CancellationToken cancellationToken)
	{
		var statusCode = (int)httpResponseMessage.StatusCode;
		if (statusCode == 429)
		{
			await BackOffAsync(httpMethod, subUrl, httpResponseMessage, prefix, state, cancellationToken).ConfigureAwait(false);
			return;
		}

		if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
		{
			_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
			await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
			return;
		}

		var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		// HTTP 400 - 499 are never retried
		if (retryServerErrors && statusCode is not (>= 400 and < 500))
		{
			_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
			_logger.LogDebug("{Prefix} failed on attempt {FailureCount}: {ResponseBody}", prefix, ++state.FailureCount, responseBody);

			if (state.FailureCount < AttemptCount)
			{
				_logger.LogDebug("{Prefix} Retrying..", prefix);
				await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
				return;
			}
		}

		_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}", prefix, httpResponseMessage.StatusCode, responseBody);
		throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
	}

	// We have been told to back off: wait for the rate limit window, up to the configured maximum.
	private async Task BackOffAsync(
		HttpMethod httpMethod,
		string subUrl,
		HttpResponseMessage httpResponseMessage,
		string prefix,
		RetryState state,
		CancellationToken cancellationToken)
	{
		var limitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
		var remainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
		var windowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

		var rateLimitInformation = $"Window: {windowSeconds}s, Count: {limitCount}, Remaining {remainingCount}";
		var delayMs = 1000 * windowSeconds;
		if (windowSeconds > MaximumBackOffSeconds)
		{
			delayMs = 1000 * MaximumBackOffSeconds;
			_logger.LogDebug("{Prefix} Rate limit window of {Seconds} seconds is greater than our max of {Max} seconds, so using the max value instead.", prefix, windowSeconds, MaximumBackOffSeconds);
		}

		state.RateLimitTotalMs = await WaitForRateLimitAsync(
			prefix,
			rateLimitInformation,
			delayMs,
			++state.RateLimitAttempt,
			state.RateLimitTotalMs,
			cancellationToken).ConfigureAwait(false);
	}
}
