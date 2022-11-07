namespace LogicMonitor.Api.Extensions;

internal static class HttpClientExtensions
{
	public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent, CancellationToken cancellationToken)
	{
		var method = new HttpMethod("PATCH");
		using var request = new HttpRequestMessage(method, requestUri)
		{
			Content = iContent
		};
		return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}
}
