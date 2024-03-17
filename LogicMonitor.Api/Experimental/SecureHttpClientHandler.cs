namespace LogicMonitor.Api.Experimental;

internal class SecureHttpClientHandler(LogicMonitorClientOptions logicMonitorClientOptions) : HttpClientHandler
{
	private readonly string _accessId = logicMonitorClientOptions.AccessId;
	private readonly string _accessKey = logicMonitorClientOptions.AccessKey;

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken
	)
	{
		var subUrl = request.RequestUri.PathAndQuery.Replace("/santaba/rest/", "");
		var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		var subUrl2 = subUrl.Contains("?")
			? subUrl.Substring(0, subUrl.IndexOf("?", StringComparison.Ordinal))
			: subUrl;
		var httpVerb = request.Method.ToString().ToUpperInvariant();
		var resourcePath = $"/{subUrl2}";
		var data = request.Content == null
			? string.Empty
			: await request.Content.ReadAsStringAsync().ConfigureAwait(false);

		// Auth header
		var authHeaderValue = _accessKey.StartsWith("lmb_", StringComparison.Ordinal)
			? $"Bearer {_accessKey}"
			: $"LMv1 {_accessId}:{Api.LogicMonitorClient.GetSignature(httpVerb, epoch, data, resourcePath, _accessKey)}:{epoch}";

		request.Headers.Add("Authorization", authHeaderValue);

		return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}
}