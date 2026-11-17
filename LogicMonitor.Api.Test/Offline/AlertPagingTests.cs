using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests for the alert paging shared by the resource, website and REST alert queries.
/// </summary>
public class AlertPagingTests
{
	private static string Items(int from, int count)
		=> "[" + string.Join(",", Enumerable.Range(from, count).Select(i => $$"""{"id":"LMD{{i}}"}""")) + "]";

	private static HttpResponseMessage Page(string itemsJson, string searchId = "")
		=> StubHttpMessageHandler.JsonResponse(
			HttpStatusCode.OK,
			$$$"""{"status":200,"errmsg":"OK","data":{"total":-1,"searchId":"{{{searchId}}}","items":{{{itemsJson}}}}}""");

	[Fact]
	public async Task GetResourceAlertsByIdAsync_RequestsTheResourceAlertsEndpointAtTheMaximumPageSize()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Page(Items(1, 2)), () => Page("[]"));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetResourceAlertsByIdAsync(5, new AlertFilter(), TestContext.Current.CancellationToken);

		alerts.Select(a => a.Id).Should().Equal("LMD1", "LMD2");
		handler.Requests[0].Uri!.AbsolutePath.Should().EndWith("device/devices/5/alerts");
		handler.Requests[0].Uri!.Query.Should().Contain("size=300");
	}

	[Fact]
	public async Task GetResourceAlertsByIdAsync_WithATakeAbovePageSize_FetchesFurtherPagesAndStopsAtTheTake()
	{
		var handler = StubHttpMessageHandler.Sequence(
			() => Page(Items(0, 300)),
			() => Page(Items(300, 50)));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetResourceAlertsByIdAsync(5, new AlertFilter { Take = 350 }, TestContext.Current.CancellationToken);

		alerts.Should().HaveCount(350);
		handler.Requests.Should().HaveCount(2);
		handler.Requests[1].Uri!.Query.Should().Contain("offset=300").And.Contain("size=50");
	}

	[Fact]
	public async Task GetResourceAlertsByIdAsync_WithATakeBelowPageSize_AsksForOnlyThatMany()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Page(Items(0, 3)));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetResourceAlertsByIdAsync(5, new AlertFilter { Take = 3 }, TestContext.Current.CancellationToken);

		alerts.Should().HaveCount(3);
		handler.Requests.Should().ContainSingle();
		handler.Requests[0].Uri!.Query.Should().Contain("size=3");
	}

	[Fact]
	public async Task GetResourceAlertsByIdAsync_IgnoresAlertsItHasAlreadySeen()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Page(Items(0, 2)), () => Page(Items(1, 2)), () => Page("[]"));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetResourceAlertsByIdAsync(5, new AlertFilter(), TestContext.Current.CancellationToken);

		alerts.Select(a => a.Id).Should().Equal("LMD0", "LMD1", "LMD2");
	}

	[Fact]
	public async Task GetWebsiteAlertsByIdAsync_RequestsTheWebsiteAlertsEndpoint()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Page(Items(0, 1)), () => Page("[]"));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetWebsiteAlertsByIdAsync(9, new AlertFilter(), TestContext.Current.CancellationToken);

		alerts.Should().ContainSingle();
		handler.Requests[0].Uri!.AbsolutePath.Should().EndWith("website/websites/9/alerts");
	}

	[Fact]
	public async Task GetAlertsAsync_QueriesTheAlertEndpoint()
	{
		var handler = StubHttpMessageHandler.Sequence(() => Page(Items(0, 2)), () => Page("[]"));
		using var client = StubbedClient.Create(handler);

		var alerts = await client.GetAlertsAsync(new AlertFilter(), TestContext.Current.CancellationToken);

		alerts.Should().HaveCount(2);
		handler.Requests[0].Uri!.AbsolutePath.Should().EndWith("alert/alerts");
	}

	[Fact]
	public async Task GetAlertsAsync_WithAMonitorObjectId_ResolvesItToTheResourceDisplayName()
	{
		var handler = new StubHttpMessageHandler(request => request.Uri!.AbsolutePath.EndsWith("device/devices/42", StringComparison.Ordinal)
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, """{"status":200,"errmsg":"OK","data":{"id":42,"displayName":"server42"}}""")
			: Page("[]"));
		using var client = StubbedClient.Create(handler);

		await client.GetAlertsAsync(new AlertFilter { MonitorObjectId = 42 }, TestContext.Current.CancellationToken);

		handler.Requests[^1].Uri!.Query.Should().Contain("server42");
	}

	[Fact]
	public async Task GetAlertsAsync_WhenOnlyWebsiteAlertsAreWanted_ResolvesTheMonitorObjectIdToTheWebsiteName()
	{
		var handler = new StubHttpMessageHandler(request => request.Uri!.AbsolutePath.EndsWith("website/websites/7", StringComparison.Ordinal)
			? StubHttpMessageHandler.JsonResponse(HttpStatusCode.OK, """{"status":200,"errmsg":"OK","data":{"id":7,"name":"site7"}}""")
			: Page("[]"));
		using var client = StubbedClient.Create(handler);

		await client.GetAlertsAsync(
			new AlertFilter { MonitorObjectId = 7, AlertTypes = [AlertType.Website] },
			TestContext.Current.CancellationToken);

		handler.Requests[^1].Uri!.Query.Should().Contain("site7");
	}

	[Fact]
	public async Task GetResourceAlertsByIdChunkedAsync_RequestsEachChunkAndMergesTheDistinctAlerts()
	{
		var handler = new StubHttpMessageHandler(request => request.Uri!.Query.Contains("offset=0", StringComparison.Ordinal) ? Page(Items(0, 2)) : Page("[]"));
		using var client = StubbedClient.Create(handler);
		var filter = new AlertFilter
		{
			StartEpochIsAfter = 1_700_000_000,
			StartEpochIsBefore = 1_700_000_000 + (3 * 3600)
		};

		var alerts = await client.GetResourceAlertsByIdChunkedAsync(5, filter, TimeSpan.FromHours(1), TestContext.Current.CancellationToken);

		alerts.Select(a => a.Id).Order().Should().Equal("LMD0", "LMD1");
		handler.Requests.Count.Should().BeGreaterThanOrEqualTo(3);
		filter.StartEpochIsAfter.Should().Be(1_700_000_000);
	}
}
