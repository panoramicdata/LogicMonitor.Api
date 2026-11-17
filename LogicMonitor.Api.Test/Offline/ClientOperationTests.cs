using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests for client operations built on the request pipeline: updates, property
/// setters, LM Logs queries and access log queries.
/// </summary>
public class ClientOperationTests
{
	private const string Ok = """{"status":200,"errmsg":"OK","data":{}}""";

	private static string Envelope(string dataJson)
		=> $$$"""{"status":200,"errmsg":"OK","data":{{{dataJson}}}}""";

	[Fact]
	public async Task PutAsync_SendsTheSerialisedItemWithAPutRequest()
	{
		var handler = StubHttpMessageHandler.Json(Ok);
		using var client = StubbedClient.Create(handler);

		await client.PutAsync("device/groups/3", new { name = "renamed" }, TestContext.Current.CancellationToken);

		var request = handler.Requests.Single();
		request.Method.Should().Be(HttpMethod.Put);
		request.Body.Should().Be("""{"name":"renamed"}""");
	}

	[Fact]
	public async Task PutAsync_OnAClientError_Throws()
	{
		var handler = StubHttpMessageHandler.Sequence(() => StubHttpMessageHandler.JsonResponse(HttpStatusCode.BadRequest, "bad"));
		using var client = StubbedClient.Create(handler);

		var act = () => client.PutAsync("device/groups/3", new { name = "renamed" }, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>()).Which.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task SetWebsiteCustomPropertyAsync_ForANewProperty_AddsItAndPutsTheWebsite()
	{
		var handler = new StubHttpMessageHandler(request => request.Method == HttpMethod.Get
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"id":3,"properties":[]}"""))
			: StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Ok));
		using var client = StubbedClient.Create(handler);

		await client.SetWebsiteCustomPropertyAsync(3, "colour", "red", SetPropertyMode.Automatic, TestContext.Current.CancellationToken);

		var put = handler.Requests.Single(r => r.Method == HttpMethod.Put);
		put.Body.Should().Contain("\"name\":\"colour\"").And.Contain("\"value\":\"red\"");
	}

	[Fact]
	public async Task SetWebsiteCustomPropertyAsync_ForAnExistingProperty_ReplacesItsValue()
	{
		var handler = new StubHttpMessageHandler(request => request.Method == HttpMethod.Get
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"id":3,"properties":[{"name":"colour","value":"red"}]}"""))
			: StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Ok));
		using var client = StubbedClient.Create(handler);

		await client.SetWebsiteCustomPropertyAsync(3, "colour", "blue", SetPropertyMode.Update, TestContext.Current.CancellationToken);

		var put = handler.Requests.Single(r => r.Method == HttpMethod.Put);
		put.Body.Should().Contain("\"value\":\"blue\"").And.NotContain("red");
	}

	[Fact]
	public async Task SetWebsiteCustomPropertyAsync_ForAnExistingPropertyWithANullValue_RemovesIt()
	{
		var handler = new StubHttpMessageHandler(request => request.Method == HttpMethod.Get
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"id":3,"properties":[{"name":"colour","value":"red"}]}"""))
			: StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Ok));
		using var client = StubbedClient.Create(handler);

		await client.SetWebsiteCustomPropertyAsync(3, "colour", null, SetPropertyMode.Delete, TestContext.Current.CancellationToken);

		handler.Requests.Single(r => r.Method == HttpMethod.Put).Body.Should().NotContain("colour");
	}

	[Fact]
	public async Task SetWebsiteCustomPropertyAsync_DeletingAMissingProperty_Throws()
	{
		var handler = StubHttpMessageHandler.Json(Envelope("""{"id":3,"properties":[]}"""));
		using var client = StubbedClient.Create(handler);

		var act = () => client.SetWebsiteCustomPropertyAsync(3, "colour", null, SetPropertyMode.Delete, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<LogicMonitorApiException>();
	}

	[Theory]
	[InlineData(SetPropertyMode.Create, null)]
	[InlineData(SetPropertyMode.Update, null)]
	public async Task SetWebsiteCustomPropertyAsync_CreatingOrUpdatingWithANullValue_Throws(SetPropertyMode mode, string? value)
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(Ok));

		var act = () => client.SetWebsiteCustomPropertyAsync(3, "colour", value, mode, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task SetWebsiteCustomPropertyAsync_DeletingWithAValue_Throws()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(Ok));

		var act = () => client.SetWebsiteCustomPropertyAsync(3, "colour", "red", SetPropertyMode.Delete, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<ArgumentException>();
	}

	[Fact]
	public async Task GetLogsAsync_SubmitsThenPollsAndTrimsToTheRequestedSize()
	{
		var calls = 0;
		var handler = new StubHttpMessageHandler(_ => calls++ == 0
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"queryId":"q1"}"""))
			: StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"queryId":"q1","progress":1.0,"logs":[{"message":"a"},{"message":"b"},{"message":"c"}]}""")));
		using var client = StubbedClient.Create(handler);

		var result = await client.GetLogsAsync(new LogQueryRequest { Size = 2 }, TestContext.Current.CancellationToken);

		result.Logs.Should().HaveCount(2);
		handler.Requests[0].Uri!.AbsolutePath.Should().EndWith("log/search");
		handler.Requests[1].Uri!.AbsolutePath.Should().EndWith("log/search/q1");
	}

	[Fact]
	public async Task GetLogsAsync_WhenTheSubmitReturnsNoQueryId_Throws()
	{
		using var client = StubbedClient.Create(StubHttpMessageHandler.Json(Envelope("{}")));

		var act = () => client.GetLogsAsync(new LogQueryRequest(), TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task GetLogsAsync_WhenTheSearchNeverFinishes_ReturnsThePartialResultAtTheTimeout()
	{
		var calls = 0;
		var handler = new StubHttpMessageHandler(_ => calls++ == 0
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"queryId":"q1"}"""))
			: StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, Envelope("""{"queryId":"q1","progress":0.2}""")));
		using var client = StubbedClient.Create(handler);

		var result = await client.GetLogsAsync(new LogQueryRequest(), TimeSpan.Zero, TestContext.Current.CancellationToken);

		result.Progress.Should().Be(0.2);
		result.Logs.Should().BeNull();
	}

	[Fact]
	public async Task GetLogItemsAsync_RequestsTheAccessLogsWithTheDateRangeAndTextFilters()
	{
		var handler = StubHttpMessageHandler.Json(Envelope("""{"total":0,"items":[]}"""));
		using var client = StubbedClient.Create(handler);
		var filter = new LogFilter(
			0,
			10,
			new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Utc),
			LogFilterSortOrder.HappenedOnAsc)
		{
			UsernameFilter = "bob",
			TextFilter = "login"
		};

		var items = await client.GetLogItemsAsync(filter, TestContext.Current.CancellationToken);

		items.Should().BeEmpty();
		var query = handler.Requests.Single().Uri!.Query;
		query.Should().Contain("size=10").And.Contain("username").And.Contain("bob").And.Contain("_all");
	}

	[Fact]
	public async Task GetLogItemsAsync_WithATakeAbovePageSize_AsksForPagesOfTheMaximumSize()
	{
		var handler = StubHttpMessageHandler.Json(Envelope("""{"total":0,"items":[]}"""));
		using var client = StubbedClient.Create(handler);
		var filter = new LogFilter(
			0,
			500,
			new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Utc),
			LogFilterSortOrder.HappenedOnAsc);

		await client.GetLogItemsAsync(filter, TestContext.Current.CancellationToken);

		handler.Requests.Single().Uri!.Query.Should().Contain("size=49");
	}
}
