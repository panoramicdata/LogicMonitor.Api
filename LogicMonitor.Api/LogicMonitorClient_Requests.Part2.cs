namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{

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
