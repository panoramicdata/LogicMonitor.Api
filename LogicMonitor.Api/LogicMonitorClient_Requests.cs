namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	/// Gets an integer header
	/// </summary>
	/// <param name="httpMethod"></param>
	/// <param name="subUrl"></param>
	/// <param name="httpStatusCode"></param>
	/// <param name="httpResponseMessage">The response message</param>
	/// <param name="header">The required header</param>
	/// <exception cref="LogicMonitorApiException">Thrown when the header is not a single integer.</exception>
	/// <returns>The integer value</returns>
	private async Task<int> GetIntegerHeaderAsync(HttpMethod httpMethod, string subUrl, HttpStatusCode httpStatusCode, HttpResponseMessage httpResponseMessage, string header)
	{
		var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

		if (!httpResponseMessage.Headers.TryGetValues(header, out var valueStringEnumerable))
		{
			var message = $"Response header '{header}' is not an integer.";
			_logger.LogDebug("{Message}", message);
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		var valueStringList = valueStringEnumerable.ToList();

		var firstOrDefaultMatchingHeader = valueStringList.FirstOrDefault();

		if (firstOrDefaultMatchingHeader is null)
		{
			var message = $"'{header}' header value contains {valueStringList.Count} values.  Expecting just one.";
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		if (!int.TryParse(firstOrDefaultMatchingHeader, out var valueInt))
		{
			var message = $"'{header}' header value '{firstOrDefaultMatchingHeader}' is not an integer.";
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		return valueInt;
	}

	private string RequestUri(string subUrl) => subUrl is "log/ingest" or "metric/ingest"
		? $"https://{AccountName}.logicmonitor.com/rest/{subUrl}"
		: $"https://{AccountName}.logicmonitor.com/santaba/rest/{subUrl}";

	private async Task<HttpResponseMessage> GetHttpResponseMessage(HttpRequestMessage requestMessage, string subUrl, string? data, CancellationToken cancellationToken)
	{
		ReadOnlyGuard.ThrowIfWrite(_options, requestMessage.Method, subUrl);

		var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		var subUrl2 = subUrl.Contains('?')
			? subUrl.Substring(0, subUrl.IndexOf('?'))
			: subUrl;
		var httpVerb = requestMessage.Method.ToString().ToUpperInvariant();
		var resourcePath = $"/{subUrl2}";
		data ??= string.Empty;

		// Auth header
		var authHeaderValue =
			_accessKey.StartsWith("lmb_", StringComparison.Ordinal)
				? $"Bearer {_accessKey}"
				: $"LMv1 {_accessId}:{GetSignature(httpVerb, epoch, data, resourcePath, _accessKey)}:{epoch}";

		requestMessage.Headers.Add("Authorization", authHeaderValue);

		_logger.LogHttpHeaders(true, "", requestMessage.Headers);
		var response = await _client
			.SendAsync(requestMessage, cancellationToken)
			.ConfigureAwait(false);

		UpdateSummary(requestMessage, response);

		return response;
	}

	private static readonly Dictionary<string, Action<Statistics>> _methodCounters = new(StringComparer.Ordinal)
	{
		["DELETE"] = s => s.ApiCallDeleteCount++,
		["GET"] = s => s.ApiCallGetCount++,
		["POST"] = s => s.ApiCallPostCount++,
		["TRACE"] = s => s.ApiCallTraceCount++,
		["HEAD"] = s => s.ApiCallHeadCount++,
		["PUT"] = s => s.ApiCallPutCount++,
		["OPTIONS"] = s => s.ApiCallOptionsCount++,
		["PATCH"] = s => s.ApiCallPatchCount++
	};

	private void UpdateSummary(HttpRequestMessage requestMessage, HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			Statistics.ApiCallSuccessCount++;
		}
		else
		{
			Statistics.ApiCallFailureCount++;
		}

		if (_methodCounters.TryGetValue(requestMessage.Method.ToString(), out var count))
		{
			count(Statistics);
		}
		else
		{
			Statistics.ApiCallOtherCount++;
		}

		Statistics.DataTransferUplinkBytes += requestMessage.Content?.Headers.ContentLength ?? 0;
		Statistics.DataTransferDownlinkBytes += response.Content?.Headers.ContentLength ?? 0;
	}

	private Task<Page<T>> FilteredGetAsync<T>(string subUrl, Filter<T>? filter, CancellationToken cancellationToken) where T : new()
		=> GetAsync<Page<T>>(UseCache, $"{subUrl}?{filter}", cancellationToken);


	/// <summary>
	///     Gets a filtered page of items
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <typeparam name="T">The item type</typeparam>
	/// <returns>The filtered list</returns>
	public virtual Task<Page<T>> GetAsync<T>(Filter<T>? filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetAsync<Page<T>>(UseCache, $"{new T().Endpoint()}?{filter}", cancellationToken);

	private Task<T> GetBySubUrlAsync<T>(string subUrl, CancellationToken cancellationToken) where T : class, new()
	=> GetAsync<T>(UseCache, subUrl, cancellationToken);

	/// <summary>
	/// Gets a JObject directly from the API
	/// </summary>
	/// <param name="subUrl">The subUrl</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<JObject> GetJObjectAsync(string subUrl, CancellationToken cancellationToken)
	=> GetAsync<JObject>(UseCache, subUrl, cancellationToken);

	/// <summary>
	/// Delete an item
	/// </summary>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public async Task DeleteAsync(string subUrl, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Delete;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, null, prefix, true, cancellationToken).ConfigureAwait(false);

		var jsonString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		if (jsonString.Contains("group is not empty"))
		{
			_logger.LogDebug("{Prefix} failed - group is not empty for suburl", prefix);
			throw new LogicMonitorApiException(httpMethod, subUrl, HttpStatusCode.PreconditionFailed, jsonString, $"{prefix} failed - group is not empty for suburl");
		}
	}

	private static string GetPrefix(HttpMethod method) => $"{Guid.NewGuid()}: {method}";

	/// <summary>
	///     Async Get method
	/// </summary>
	/// <param name="permitCacheIfEnabled"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T"></typeparam>
	/// <exception cref="LogicMonitorApiException"></exception>
	private async Task<T> GetAsync<T>(bool permitCacheIfEnabled, string subUrl, CancellationToken cancellationToken) where T : class, new()
	{
		var httpMethod = HttpMethod.Get;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		// Age the Cache
		_cache.Age();

		var stopwatch = Stopwatch.StartNew();

		var useCache = permitCacheIfEnabled && UseCache;
		if (useCache && _cache.TryGetValue<T>(subUrl, out var cacheObject))
		{
			_logger.LogDebug("{Prefix} complete (from cache: {ElapsedMilliseconds:N0}ms)",
				prefix,
				stopwatch.ElapsedMilliseconds);
			if (cacheObject is not null)
			{
				return (T)cacheObject;
			}
		}

		var httpResponseMessage = await SendGetWithRetryAsync(subUrl, prefix, stopwatch, cancellationToken).ConfigureAwait(false);

		// File and raw responses are returned as they are, and never cached
		var rawResponse = await ReadRawResponseAsync<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
		if (rawResponse is not null)
		{
			return rawResponse;
		}

		var deserializedObject = await ReadPortalResponseAsync<T>(httpResponseMessage, subUrl, prefix, cancellationToken).ConfigureAwait(false);

		// Cache the result
		if (useCache && deserializedObject is not null)
		{
			_cache.AddOrUpdate(subUrl, deserializedObject);
		}

		return deserializedObject ?? new();
	}

	private async Task<T?> ReadRawResponseAsync<T>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken) where T : class
	{
		if (typeof(T).Name == nameof(XmlResponse))
		{
			var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogTrace("RESPONSE:\r\n\r\n {Content}", content);
			return new XmlResponse { Content = content } as T
				?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		if (typeof(T) == typeof(List<byte>))
		{
			var content = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogTrace("Byte Array");
			return content.ToList() as T
				?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		if (typeof(T) == typeof(DownloadFileInfo))
		{
			var tempFileInfo = new FileInfo(Path.GetTempFileName());
			using Stream output = File.OpenWrite(tempFileInfo.FullName);
			using var input = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
			await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);

			return new DownloadFileInfo
			{
				FileInfo = tempFileInfo
			} as T
			?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		return null;
	}

	private async Task<T?> ReadPortalResponseAsync<T>(HttpResponseMessage httpResponseMessage, string subUrl, string prefix, CancellationToken cancellationToken) where T : class, new()
	{
		var portalResponse = new PortalResponse<T>(httpResponseMessage);

		if (!portalResponse.IsSuccessStatusCode)
		{
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
			_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
				prefix,
				httpResponseMessage.StatusCode,
				responseBody);
			_logger.LogTrace("RESPONSE:\r\n\r\n{ResponseBody}", responseBody);
			throw new LogicMonitorApiException(HttpMethod.Get, subUrl, portalResponse.HttpStatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
		}

		_logger.LogTrace("RESPONSE:\r\n\r\n{Data}:", portalResponse.Data?.ToString());
		try
		{
			return portalResponse.GetObject(_jsonConverters);
		}
		catch (DeserializationException e)
		{
			_logger.LogError(e, "{Prefix} Unable to deserialize\n{ResponseBody}\n{Message}",
				prefix,
				e.ResponseBody,
				e.Message);
			throw;
		}
	}
}
