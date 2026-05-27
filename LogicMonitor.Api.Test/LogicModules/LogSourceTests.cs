namespace LogicMonitor.Api.Test.LogicModules;

public class LogSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllLogSources()
	{
		var logSources = await LogicMonitorClient
			.GetAllAsync<LogSource>(CancellationToken);

		logSources.Should().NotBeNull();
	}

	[Fact]
	public async Task GetLogSourceById_WhenAvailable()
	{
		var logSources = await LogicMonitorClient
			.GetAllAsync<LogSource>(CancellationToken);

		if (logSources.Count == 0)
		{
			return;
		}

		var logSource = await LogicMonitorClient
			.GetAsync<LogSource>(logSources[0].Id, CancellationToken);

		logSource.Should().NotBeNull();
		logSource.Id.Should().Be(logSources[0].Id);
	}
}