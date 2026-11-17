using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests for the request pipeline shared by every client call: signing, retries,
/// rate limiting, error mapping and caching.
/// </summary>
public class LogicMonitorClientPipelineTests
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

	private static HttpResponseMessage Status(HttpStatusCode statusCode, string body = "{}")
		=> StubHttpMessageHandler.JsonResponse(statusCode, body);

	private static HttpResponseMessage Ok(string body = EmptyPage)
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
			() => Status(HttpStatusCode.BadGateway),
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
		handler.Requests[1].Uri!.Query.Should().Contain("offset=300");
	}

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

	[Fact]
	public async Task GetByNameAsync_WithSeveralMatches_Throws()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(StubbedClient.PageJson("""[{"id":4},{"id":5}]""", 2)));

		var act = () => client.GetByNameAsync<ResourceGroup>("x", TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task DeleteAsync_SendsADeleteRequest()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{}}""");
		using var client = StubbedClient.Create(handler);

		await client.DeleteAsync("device/groups/3", TestContext.Current.CancellationToken);

		var request = handler.Requests.Should().ContainSingle().Subject;
		request.Method.Should().Be(HttpMethod.Delete);
		request.Uri!.AbsolutePath.Should().EndWith("device/groups/3");
	}

	[Fact]
	public async Task DeleteAsync_OnAClientError_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.Conflict, "nope"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.DeleteAsync("device/groups/3", TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.Conflict);
	}

	[Fact]
	public async Task DeleteAsync_WhenRateLimited_WaitsAndThenSucceeds()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(), () => Ok("{}"));
		using var client = StubbedClient.Create(handler);

		await client.DeleteAsync("device/groups/3", TestContext.Current.CancellationToken);

		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task DeleteAsync_WhenTheGroupIsNotEmpty_ThrowsPreconditionFailed()
	{
		var handler = StubHttpMessageHandler.Json("""{"errmsg":"group is not empty"}""");
		using var client = StubbedClient.Create(handler);

		var act = () => client.DeleteAsync("device/groups/3", TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.PreconditionFailed);
	}

	[Fact]
	public async Task DeleteAsync_AfterATransportFailure_Retries()
	{
		var calls = 0;
		var handler = new StubHttpMessageHandler(_ => calls++ == 0
			? throw new HttpRequestException("flaky")
			: Ok("{}"));
		using var client = StubbedClient.Create(handler);

		await client.DeleteAsync("device/groups/3", TestContext.Current.CancellationToken);

		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task PostAsync_SendsTheSerialisedBodyAndReturnsTheDeserialisedResult()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"id":11,"name":"Made"}}""");
		using var client = StubbedClient.Create(handler);

		var created = await client.PostAsync<ResourceGroupCreationDto, ResourceGroup>(
			new ResourceGroupCreationDto { Name = "Made", ParentId = "1" },
			"device/groups",
			TestContext.Current.CancellationToken);

		created.Id.Should().Be(11);
		var request = handler.Requests.Single();
		request.Method.Should().Be(HttpMethod.Post);
		request.Body.Should().Contain("\"name\":\"Made\"");
	}

	[Fact]
	public async Task PostAsync_OnAClientError_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.BadRequest, "bad"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.PostAsync<ResourceGroupCreationDto, ResourceGroup>(
			new ResourceGroupCreationDto(),
			"device/groups",
			TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task PostAsync_WhenRateLimited_WaitsAndThenSucceeds()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(), () => Ok("""{"status":200,"errmsg":"OK","data":{"id":2}}"""));
		using var client = StubbedClient.Create(handler);

		var created = await client.PostAsync<ResourceGroupCreationDto, ResourceGroup>(
			new ResourceGroupCreationDto(),
			"device/groups",
			TestContext.Current.CancellationToken);

		created.Id.Should().Be(2);
		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task PostJObjectAsync_SendsTheObjectAndReturnsTheResponseObject()
	{
		var handler = StubHttpMessageHandler.Json("""{"status":200,"errmsg":"OK","data":{"echo":true}}""");
		using var client = StubbedClient.Create(handler);

		var result = await client.PostJObjectAsync(new JObject { ["a"] = 1 }, "some/endpoint", TestContext.Current.CancellationToken);

		result!["echo"]!.Value<bool>().Should().BeTrue();
		handler.Requests.Single().Body.Should().Contain("\"a\": 1");
	}

	[Fact]
	public async Task PostJObjectAsync_OnAClientError_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.Forbidden, "no"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.PostJObjectAsync(new JObject(), "some/endpoint", TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.Forbidden);
	}

	[Fact]
	public async Task PostJObjectAsync_WhenRateLimited_WaitsAndThenSucceeds()
	{
		var handler = StubHttpMessageHandler.Sequence(() => RateLimited(), () => Ok("""{"status":200,"errmsg":"OK","data":{}}"""));
		using var client = StubbedClient.Create(handler);

		await client.PostJObjectAsync(new JObject(), "some/endpoint", TestContext.Current.CancellationToken);

		handler.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task PatchAsync_SendsThePatchFieldsOnTheQueryString()
	{
		var handler = StubHttpMessageHandler.Json("{}");
		using var client = StubbedClient.Create(handler);

		await client.PatchAsync(
			"device/devices/5",
			new Dictionary<string, object> { ["description"] = "d", ["name"] = "n" },
			TestContext.Current.CancellationToken);

		var request = handler.Requests.Single();
		request.Method.Method.Should().Be("PATCH");
		request.Uri!.Query.Should().Be("?patchFields=description,name");
		request.Body.Should().Contain("\"description\":\"d\"");
	}

	[Fact]
	public async Task PatchAsync_WhenForbidden_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.Forbidden, "no"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.PatchAsync("device/devices/5", new Dictionary<string, object> { ["name"] = "n" }, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.Forbidden);
	}

	[Fact]
	public async Task PatchAsync_OnAnyOtherFailure_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Status(HttpStatusCode.InternalServerError, "boom"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.PatchAsync("device/devices/5", new Dictionary<string, object> { ["name"] = "n" }, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
	}

	[Fact]
	public async Task AnyRequest_StampsTheStatistics()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Ok("{}"), () => Status(HttpStatusCode.NotFound));
		using var client = StubbedClient.Create(handler);

		await client.DeleteAsync("device/groups/1", TestContext.Current.CancellationToken);
		var act = () => client.DeleteAsync("device/groups/2", TestContext.Current.CancellationToken);
		await act.Should().ThrowAsync<LogicMonitorApiException>();

		client.Statistics.ApiCallSuccessCount.Should().Be(1);
		client.Statistics.ApiCallFailureCount.Should().Be(1);
		client.Statistics.ApiCallDeleteCount.Should().Be(2);
	}

	[Fact]
	public async Task ReadOnlyClient_RefusesWritesBeforeAnyRequestIsSent()
	{
		var handler = StubHttpMessageHandler.Json("{}");
		using var client = StubbedClient.Create(handler, o => o.IsReadOnly = true);

		var act = () => client.DeleteAsync("device/groups/1", TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
		handler.Requests.Should().BeEmpty();
	}

	[Fact]
	public void AttemptCount_BelowOne_IsRejected()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json("{}"));

		var act = () => client.AttemptCount = 0;

		act.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void Constructor_WithoutAnAccount_Throws()
	{
		var act = () => new LogicMonitorClient(new LogicMonitorClientOptions { Account = null! });

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void TimeOutAndCacheTimeSpan_RoundTripThroughTheClient()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json("{}"));

		client.TimeOut = TimeSpan.FromSeconds(7);
		client.CacheTimeSpan = TimeSpan.FromMinutes(2);

		client.TimeOut.Should().Be(TimeSpan.FromSeconds(7));
		client.CacheTimeSpan.Should().Be(TimeSpan.FromMinutes(2));
		client.AccountName.Should().Be("acme");
	}
}
