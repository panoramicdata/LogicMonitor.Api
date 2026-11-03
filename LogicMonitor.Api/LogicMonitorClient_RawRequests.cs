namespace LogicMonitor.Api;

/// <summary>
///     Requests whose body is not a JSON object: top-level arrays, bare values and file uploads
/// </summary>
public partial class LogicMonitorClient
{
	private const string MultipartBoundary = "LogicMonitorApiBoundary";

	/// <summary>
	///     Sends a request and returns the response body unparsed
	/// </summary>
	/// <param name="httpMethod">The HTTP method</param>
	/// <param name="subUrl">The subURL</param>
	/// <param name="data">The request body</param>
	/// <param name="contentType">The request body content type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <param name="signedData">What the LMv1 signature covers, if not the whole body</param>
	private async Task<string> SendRawAsync(
		HttpMethod httpMethod,
		string subUrl,
		string? data,
		string contentType,
		CancellationToken cancellationToken,
		string? signedData = null)
	{
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);
		var rateLimitAttempt = 0;
		var rateLimitTotalMs = 0L;
		while (true)
		{
			using var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl));
			if (data is not null)
			{
				// The body is signed as sent, so the content type must not add a charset parameter
				requestMessage.Content = new StringContent(data, Encoding.UTF8);
				requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
			}

			using var httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, signedData ?? data, cancellationToken)
				.ConfigureAwait(false);
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return responseBody;
			}

			if ((int)httpResponseMessage.StatusCode != 429)
			{
				throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
			}

			var windowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);
			rateLimitTotalMs = await WaitForRateLimitAsync(
				prefix,
				$"Window: {windowSeconds}s",
				1000 * Math.Min(windowSeconds, MaximumBackOffSeconds),
				++rateLimitAttempt,
				rateLimitTotalMs,
				cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Sends a JSON request and deserializes a response that may be a JSON array or a bare value
	/// </summary>
	private async Task<T?> SendRawJsonAsync<T>(
		HttpMethod httpMethod,
		string subUrl,
		object? body,
		CancellationToken cancellationToken)
	{
		var data = body is null
			? null
			: JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		var responseBody = await SendRawAsync(httpMethod, subUrl, data, "application/json", cancellationToken).ConfigureAwait(false);
		return string.IsNullOrWhiteSpace(responseBody)
			? default
			: JsonConvert.DeserializeObject<T>(responseBody);
	}

	/// <summary>
	///     Uploads a text file as multipart/form-data in a field named "file"
	/// </summary>
	/// <remarks>LogicMonitor signs only the file content of an upload, not the multipart envelope.</remarks>
	private async Task<string> UploadFileAsync(
		string subUrl,
		string fileName,
		string fileContent,
		string fileContentType,
		CancellationToken cancellationToken)
	{
		var data =
			$"--{MultipartBoundary}\r\n" +
			$"Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\"\r\n" +
			$"Content-Type: {fileContentType}\r\n" +
			"\r\n" +
			$"{fileContent}\r\n" +
			$"--{MultipartBoundary}--\r\n";
		return await SendRawAsync(HttpMethod.Post, subUrl, data, $"multipart/form-data; boundary={MultipartBoundary}", cancellationToken, fileContent).ConfigureAwait(false);
	}
}
