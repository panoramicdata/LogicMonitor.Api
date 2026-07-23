namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceGraphWriteTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task UpdateDataSourceGraph_NoOpRoundTrip_Succeeds()
	{
		// Find a DataSource that has at least one graph. Enumerate ids via a raw JObject query to
		// avoid deserialising full DataSource objects (which can throw on newer server-only fields).
		var page = await LogicMonitorClient
			.GetJObjectAsync("setting/datasources?fields=id&size=50", CancellationToken);

		var ids = (page["items"] as JArray)?
			.Select(i => i["id"]?.Value<int>() ?? 0)
			.Where(id => id > 0)
			.ToList() ?? [];

		foreach (var dataSourceId in ids)
		{
			var graphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSourceId, CancellationToken);
			if (graphs.Count == 0)
			{
				continue;
			}

			var graph = graphs[0];

			// Re-fetch the full graph, then PUT it back unchanged — a no-op that exercises the
			// update path without altering the portal.
			var full = await LogicMonitorClient
				.GetDataSourceGraphAsync(dataSourceId, graph.Id, CancellationToken);

			await LogicMonitorClient
				.UpdateDataSourceGraphAsync(dataSourceId, full, CancellationToken);

			return; // one successful round-trip is enough
		}

		Assert.Skip("No DataSource with graphs was available on the test portal.");
	}
}
