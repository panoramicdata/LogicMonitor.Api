using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests for the request pipeline shared by every client call: signing, retries,
/// rate limiting, error mapping and caching.
/// </summary>
public partial class LogicMonitorClientPipelineTests
{

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
		var handler = StubHttpMessageHandler.Sequence(() => Ok("{}"), () => Status(HttpStatusCode.NotFound, "{}"));
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
