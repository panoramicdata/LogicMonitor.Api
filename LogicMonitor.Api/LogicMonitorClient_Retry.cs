namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	/// Post an item
	/// </summary>
	/// <typeparam name="TIn">The posted object type</typeparam>
	/// <typeparam name="TOut">The returned object type</typeparam>
	/// <param name="postedObject">The posted object </param>
	/// <param name="subUrl">The endpoint</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task<TOut> PostAsync<TIn, TOut>(TIn postedObject, string subUrl, CancellationToken cancellationToken) where TOut : new()
	{
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		// LMREP-1042: "d:\"EBSDB [prod24778]\" does not work, however "d:\"EBSDB *prod24778*\" matches. Unrelated to URl encoding, etc...
		var data = JsonConvert.SerializeObject(postedObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, data, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<TOut>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		var deserializedObject = portalResponse.GetObject();
		return deserializedObject != null ? deserializedObject : new();
	}

	/// <summary>
	/// Post a jObect
	/// </summary>
	/// <param name="jObject">The posted jObject</param>
	/// <param name="subUrl">The endpoint</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="LogicMonitorApiException"></exception>
	public async Task<JObject?> PostJObjectAsync(
		JObject jObject,
		string subUrl,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var data = jObject.ToString();
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, data, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<JObject>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		return portalResponse.GetObject();
	}

	private sealed class RetryState
	{
		public int FailureCount { get; set; }

		public int RateLimitAttempt { get; set; }

		public long RateLimitTotalMs { get; set; }
	}

	private async Task<HttpResponseMessage> SendOnceAsync(HttpMethod httpMethod, string subUrl, string? data, CancellationToken cancellationToken)
	{
		using var content = data is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
		using var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)) { Content = content };
		return await GetHttpResponseMessage(requestMessage, subUrl, data, cancellationToken).ConfigureAwait(false);
	}

	private async Task<HttpResponseMessage> SendWithRetryAsync(
		HttpMethod httpMethod,
		string subUrl,
		string? data,
		string prefix,
		bool retryTransportFailures,
		CancellationToken cancellationToken)
	{
		var state = new RetryState();
		while (true)
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await SendOnceAsync(httpMethod, subUrl, data, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception e) when (retryTransportFailures && LogTransportFailure(e, prefix, state))
			{
				continue;
			}

			_logger.LogDebug("{Prefix} complete", prefix);
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return httpResponseMessage;
			}

			await WaitBeforeRetryOrThrowAsync(httpMethod, subUrl, httpResponseMessage, prefix, state, false, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task<HttpResponseMessage> SendGetWithRetryAsync(string subUrl, string prefix, Stopwatch stopwatch, CancellationToken cancellationToken)
	{
		var state = new RetryState();
		while (true)
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await SendOnceAsync(HttpMethod.Get, subUrl, null, cancellationToken).ConfigureAwait(false);
				_logger.LogDebug("{Prefix} complete (from remote: {ElapsedMilliseconds:N0}ms)",
					prefix,
					stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e) when (LogTransportFailure(e, prefix, state))
			{
				await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
				continue;
			}

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return httpResponseMessage;
			}

			await WaitBeforeRetryOrThrowAsync(HttpMethod.Get, subUrl, httpResponseMessage, prefix, state, true, cancellationToken).ConfigureAwait(false);
		}
	}

	// Returns true when the caller should retry; false lets the original exception propagate.
	private bool LogTransportFailure(Exception e, string prefix, RetryState state)
	{
		_logger.LogDebug(e, "{Prefix} failed on attempt {FailureCount}: {StackTrace}",
			prefix,
			++state.FailureCount,
			e.ToString());
		if (state.FailureCount < AttemptCount)
		{
			_logger.LogDebug("{Prefix} Retrying..", prefix);
			return true;
		}

		_logger.LogDebug("{Prefix} Giving up.", prefix);
		return false;
	}
}
