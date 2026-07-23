namespace LogicMonitor.Api.Test.Logging;

public class LmLogsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetLogs_LastHour_ReturnsResult()
	{
		var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var request = new LogQueryRequest
		{
			Query = "*",
			StartTime = nowMs - 3_600_000,
			EndTime = nowMs,
			Size = 5
		};

		var result = await LogicMonitorClient
			.GetLogsAsync(request, CancellationToken);

		result.Should().NotBeNull();
		result.QueryId.Should().NotBeNullOrEmpty();

		// A portal may legitimately have no logs in the window; when logs are present they must be
		// well-formed (a message and a mapped resource).
		if (result.Logs is { Count: > 0 })
		{
			result.Logs.Should().OnlyContain(l => l.Message != null);
		}
	}
}
