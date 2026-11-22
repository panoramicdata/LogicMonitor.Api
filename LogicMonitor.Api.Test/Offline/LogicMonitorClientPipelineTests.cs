using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests for the request pipeline shared by every client call: signing, retries,
/// rate limiting, error mapping and caching.
/// </summary>
public partial class LogicMonitorClientPipelineTests
{
	private const string EmptyPage = """{"status":200,"errmsg":"OK","data":{"total":0,"items":[]}}""";

	private static HttpResponseMessage RateLimited(string? window = "0")
	{
		var response = StubHttpMessageHandler.JsonResponse(HttpStatusCode.TooManyRequests, "{}");
		response.Headers.Add("X-Rate-Limit-Limit", "10");
		response.Headers.Add("X-Rate-Limit-Remaining", "0");
		if (window is not null)
		{
			response.Headers.Add("X-Rate-Limit-Window", window);
		}

		return response;
	}

	private static HttpResponseMessage Status(HttpStatusCode statusCode, string body)
		=> StubHttpMessageHandler.JsonResponse(statusCode, body);

	private static HttpResponseMessage Ok(string body)
		=> StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, body);

	[Fact]
	public async Task GetAsync_SignsTheRequestAndTargetsTheAccountHost()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"id":7,"name":"Group"}}""");
		using var client = StubbedClient.Create(handler);

		var group = await client.GetAsync<ResourceGroup>(7, TestContext.Current.CancellationToken);

		group.Id.Should().Be(7);
		var request = handler.Requests.Should().ContainSingle().Subject;
		request.Method.Should().Be(HttpMethod.Get);
		request.Uri!.ToString().Should().Be("https://acme.logicmonitor.com/santaba/rest/device/groups/7");
		request.Authorization.Should().StartWith("LMv1 id:");
	}

	[Fact]
	public async Task GetAsync_WithABearerToken_UsesTheBearerScheme()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"id":7}}""");
		using var client = StubbedClient.Create(handler, o => o.AccessKey = "lmb_token");

		await client.GetAsync<ResourceGroup>(7, TestContext.Current.CancellationToken);

		handler.Requests.Single().Authorization.Should().Be("Bearer lmb_token");
	}

	[Fact]
	public async Task GetAsync_OnAClientError_ThrowsWithoutRetrying()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.NotFound, "missing"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.GetAsync<ResourceGroup>(7, TestContext.Current.CancellationToken);

		var exception = (await act.Should().ThrowAsync<LogicMonitorApiException>()).Which;
		exception.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
		handler.Requests.Should().ContainSingle();
	}

	[Fact]
	public async Task GetAsync_OnAServerError_RetriesUntilTheAttemptCountThenThrows()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.InternalServerError, "boom"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.GetAsync<ResourceGroup>(7, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<LogicMonitorApiException>();
		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetAsync_AfterATransientServerError_ReturnsTheNextSuccess()
	{
		var handler = StubHttpMessageHandler.Sequence(
			() => Status(HttpStatusCode.BadGateway, "{}"),
			() => Ok("""{"status":200,"errmsg":"OK","data":{"id":9}}"""));
		using var client = StubbedClient.Create(handler);

		var group = await client.GetAsync<ResourceGroup>(9, TestContext.Current.CancellationToken);

		group.Id.Should().Be(9);
		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetAsync_AfterATransportFailure_RetriesAndThenThrowsTheTransportException()
	{
		var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("network down"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.GetAsync<ResourceGroup>(7, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<HttpRequestException>();
		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetAsync_WhenRateLimited_WaitsAndThenSucceeds()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(), () => Ok("""{"status":200,"errmsg":"OK","data":{"id":3}}"""));
		using var client = StubbedClient.Create(handler);

		var group = await client.GetAsync<ResourceGroup>(3, TestContext.Current.CancellationToken);

		group.Id.Should().Be(3);
		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetAsync_WhenRateLimitedWithoutAWindowHeader_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(window: null));
		using var client = StubbedClient.Create(handler);

		var act = () => client.GetAsync<ResourceGroup>(3, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.Message.Should().Contain("X-Rate-Limit-Window");
	}

	[Fact]
	public async Task GetAsync_WhenRateLimitedWithANonNumericWindow_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(window: "soon"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.GetAsync<ResourceGroup>(3, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.Message.Should().Contain("is not an integer");
	}

	[Fact]
	public async Task GetAsync_WhenRateLimitedForLongerThanTheMaximumBackOff_UsesTheMaximumInstead()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited("3600"), () => Ok("""{"status":200,"errmsg":"OK","data":{"id":3}}"""));
		using var client = StubbedClient.Create(handler, o => o.MaximumBackOffSeconds = 0);

		var group = await client.GetAsync<ResourceGroup>(3, TestContext.Current.CancellationToken);

		group.Id.Should().Be(3);
	}

	[Fact]
	public async Task GetAsync_WithTheCacheOn_ServesTheSecondCallFromTheCache()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"id":5}}""");
		using var client = StubbedClient.Create(handler);
		client.UseCache = true;

		await client.GetAsync<ResourceGroup>(5, TestContext.Current.CancellationToken);
		await client.GetAsync<ResourceGroup>(5, TestContext.Current.CancellationToken);

		handler.Requests.Should().ContainSingle();
	}

	[Fact]
	public async Task GetAsync_AfterClearingTheCache_RequestsAgain()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"id":5}}""");
		using var client = StubbedClient.Create(handler);
		client.UseCache = true;

		await client.GetAsync<ResourceGroup>(5, TestContext.Current.CancellationToken);
		client.ClearCache();
		await client.GetAsync<ResourceGroup>(5, TestContext.Current.CancellationToken);

		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetAsync_ForAnXmlResponse_ReturnsTheRawContent()
	{
		var handler = StubHttpMessageHandler.Json("<root />");
		using var client = StubbedClient.Create(handler);

		var xml = await client.GetConfigSourceXmlAsync(1, TestContext.Current.CancellationToken);

		xml.Should().Be("<root />");
	}

	[Fact]
	public async Task GetAllAsync_PagesUntilEveryItemIsReturned()
	{
		var calls = 0;
		var handler = new StubHttpMessageHandler(_ => calls++ == 0
			? Ok(StubbedClient.PageJson("""[{"id":1},{"id":2}]""", 3))
			: Ok(StubbedClient.PageJson("""[{"id":3}]""", 3)));
		using var client = StubbedClient.Create(handler);

		var groups = await client.GetAllAsync<ResourceGroup>(TestContext.Current.CancellationToken);

		groups.Select(g => g.Id).Should().Equal(1, 2, 3);
		handler.Requests.Should().HaveCount(2);
		handler.Requests[0].Uri!.Query.Should().Contain("size=1000");
		handler.Requests[1].Uri!.Query.Should().Contain("offset=2");
	}

	[Fact]
	public async Task GetAllAsync_WhenTheServerCapsBelowTheRequestedSize_SkipsNothing()
	{
		var calls = 0;
		var handler = new StubHttpMessageHandler(_ => calls++ == 0
			? Ok(StubbedClient.PageJson(ItemsJson(1, 300), 450))
			: Ok(StubbedClient.PageJson(ItemsJson(301, 150), 450)));
		using var client = StubbedClient.Create(handler);

		var groups = await client.GetAllAsync<ResourceGroup>(TestContext.Current.CancellationToken);

		groups.Select(g => g.Id).Should().Equal(Enumerable.Range(1, 450));
		handler.Requests[1].Uri!.Query.Should().Contain("offset=300").And.Contain("size=1000");
	}

	[Fact]
	public async Task GetAllAsync_WithATakeAboveTheDefaultPageSize_AsksForPagesOfTheDefaultSize()
	{
		var handler = StubHttpMessageHandler.Json(StubbedClient.PageJson(ItemsJson(1, 1), 1));
		using var client = StubbedClient.Create(handler);

		await client.GetAllAsync(new Filter<ResourceGroup> { Take = 5000 }, TestContext.Current.CancellationToken);

		handler.Requests[0].Uri!.Query.Should().Contain("size=1000");
	}

	private static string ItemsJson(int from, int count)
		=> "[" + string.Join(",", Enumerable.Range(from, count).Select(i => $$"""{"id":{{i}}}""")) + "]";

	[Fact]
	public async Task GetAllAsync_WithAnEmptyFirstPage_ReturnsNothing()
	{
		var handler = StubHttpMessageHandler.Json(EmptyPage);
		using var client = StubbedClient.Create(handler);

		var groups = await client.GetAllAsync<ResourceGroup>(TestContext.Current.CancellationToken);

		groups.Should().BeEmpty();
		handler.Requests.Should().ContainSingle();
	}

	[Fact]
	public async Task GetAllAsync_WithStrictPagingAndAMismatchedTotal_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Ok(StubbedClient.PageJson("""[{"id":1}]""", -1)), () => Ok(EmptyPage));
		using var client = StubbedClient.Create(handler);
		client.StrictPagingTotalChecking = true;

		var act = () => client.GetAllAsync<ResourceGroup>(TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<PagingException>();
	}

	[Fact]
	public async Task GetAllAsync_WithATakeInTheFilter_StopsAtThatMany()
	{
		var handler = StubHttpMessageHandler.Json(StubbedClient.PageJson("""[{"id":1},{"id":2},{"id":3}]""", 10));
		using var client = StubbedClient.Create(handler);

		var groups = await client.GetAllAsync(new Filter<ResourceGroup> { Take = 3 }, TestContext.Current.CancellationToken);

		groups.Should().HaveCount(3);
		handler.Requests.Should().ContainSingle();
	}

	[Fact]
	public async Task GetByNameAsync_WithNoMatch_ReturnsNull()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(EmptyPage));

		var group = await client.GetByNameAsync<ResourceGroup>("nothing", TestContext.Current.CancellationToken);

		group.Should().BeNull();
	}

	[Fact]
	public async Task GetByNameAsync_WithOneMatch_ReturnsIt()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(StubbedClient.PageJson("""[{"id":4,"name":"x"}]""", 1)));

		var group = await client.GetByNameAsync<ResourceGroup>("x", TestContext.Current.CancellationToken);

		group!.Id.Should().Be(4);
	}
}
