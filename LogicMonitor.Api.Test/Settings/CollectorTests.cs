using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorTests : TestWithOutput
{
	public CollectorTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task ExecuteDebugCommand()
	{
		var debugCommandResponse = await LogicMonitorClient.ExecuteDebugCommandAsync(CollectorId, "!ping 8.8.8.8", CancellationToken.None).ConfigureAwait(false);

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		debugCommandResponse.SessionId.Should().BePositive();
	}

	[Fact]
	public async Task ExecuteDebugCommandAndWaitForResult()
	{
		var debugCommandResponse = await LogicMonitorClient.ExecuteDebugCommandAndWaitForResultAsync(CollectorId, "!ping 8.8.8.8", 20000, 100, CancellationToken.None).ConfigureAwait(false);

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		debugCommandResponse.Output.Should().NotBeNullOrEmpty();
		Logger.LogInformation("{Message}", debugCommandResponse.Output);
	}

	[Fact]
	public async Task GetCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(CancellationToken.None).ConfigureAwait(false);

		// Make sure that some are returned
		collectors.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		collectors.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Get each one by id
		foreach (var collector in collectors)
		{
			var refetchedCollector = await LogicMonitorClient.GetAsync<Collector>(collector.Id, CancellationToken.None).ConfigureAwait(false);
			refetchedCollector.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetCollector()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(CancellationToken.None).ConfigureAwait(false);
		collectors.Should().NotBeNull();
		collectors.Should().NotBeNullOrEmpty();
		var refetchedCollector = await LogicMonitorClient
			.GetAsync<Collector>(collectors[0].Id, CancellationToken.None)
			.ConfigureAwait(false);

		refetchedCollector.Should().NotBeNull();
	}

	[Fact]
	public async Task Get1Collector()
	{
		var collector = await LogicMonitorClient
			.GetAsync<Collector>(CollectorId, default)
			.ConfigureAwait(false);

		collector.Should().NotBeNull();
	}
}
