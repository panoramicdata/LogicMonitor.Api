using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api.Extensions
{
	internal static class HttpClientExtensions
	{
		public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent, CancellationToken cancellationToken = default)
		{
			var method = new HttpMethod("PATCH");
			using (var request = new HttpRequestMessage(method, requestUri)
			{
				Content = iContent
			})
			{
				var response = new HttpResponseMessage();
				try
				{
					response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
				}
				catch (TaskCanceledException e)
				{
					Debug.WriteLine($"ERROR: {e}");
				}
				return response;
			}
		}
	}
}