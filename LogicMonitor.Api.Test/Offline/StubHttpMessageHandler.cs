using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// A recorded request made through <see cref="StubHttpMessageHandler"/>.
/// </summary>
/// <param name="Method">The HTTP method</param>
/// <param name="Uri">The request URI</param>
/// <param name="Body">The request body, or null when there was none</param>
/// <param name="Authorization">The Authorization header value, or null when absent</param>
public sealed record RecordedRequest(HttpMethod Method, Uri? Uri, string? Body, string? Authorization);

/// <summary>
/// An <see cref="HttpMessageHandler"/> that answers from a delegate and records what it was asked.
/// </summary>
public sealed class StubHttpMessageHandler(Func<RecordedRequest, HttpResponseMessage> respond) : HttpMessageHandler
{
	private readonly List<RecordedRequest> _requests = [];

	/// <summary>
	/// Every request seen so far, in order.
	/// </summary>
	public IReadOnlyList<RecordedRequest> Requests
	{
		get
		{
			lock (_requests)
			{
				return [.. _requests];
			}
		}
	}

	/// <summary>
	/// A handler that always answers 200 with the given JSON body.
	/// </summary>
	public static StubHttpMessageHandler Json(string content)
		=> new(_ => JsonResponse(HttpStatusCode.OK, content));

	/// <summary>
	/// A handler that answers each request from the queue in order, repeating the last answer when it runs out.
	/// </summary>
	public static StubHttpMessageHandler Sequence(params Func<HttpResponseMessage>[] responses)
	{
		var index = -1;
		return new StubHttpMessageHandler(_ => responses[Math.Min(Interlocked.Increment(ref index), responses.Length - 1)]());
	}

	/// <summary>
	/// Builds a JSON response.
	/// </summary>
	public static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string json)
		=> new(statusCode) { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var body = request.Content is null
			? null
			: await request.Content.ReadAsStringAsync(cancellationToken);
		var recorded = new RecordedRequest(
			request.Method,
			request.RequestUri,
			body,
			request.Headers.TryGetValues("Authorization", out var values) ? values.FirstOrDefault() : null);
		lock (_requests)
		{
			_requests.Add(recorded);
		}

		return respond(recorded);
	}
}
