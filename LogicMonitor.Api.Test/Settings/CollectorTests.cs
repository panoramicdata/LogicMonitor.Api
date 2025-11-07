using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task ExecuteDebugCommand()
	{
		var debugCommandResponse = await LogicMonitorClient
			.ExecuteDebugCommandAsync(
				CollectorId,
				"!ping 8.8.8.8",
				CancellationToken
			)
			;

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		debugCommandResponse.SessionId.Should().BePositive();
	}

	[Fact]
	public async Task ExecuteDebugCommandAndWaitForResult()
	{
		var debugCommandResponse = await LogicMonitorClient
			.ExecuteDebugCommandAndWaitForResultAsync(
				CollectorId,
				"!ping 8.8.8.8",
				20000,
				100,
				CancellationToken
			)
			;

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		debugCommandResponse ??= new();
		debugCommandResponse.Output.Should().NotBeNullOrEmpty();
		Logger.LogInformation("{Message}", debugCommandResponse.Output);
	}

	[Fact]
	public async Task GetCollectors()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken);

		// Make sure that some are returned
		collectors.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		collectors.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Get each one by id
		foreach (var collector in collectors)
		{
			var refetchedCollector = await LogicMonitorClient
				.GetAsync<Collector>(collector.Id, CancellationToken);
			refetchedCollector.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetCollector()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken);
		collectors.Should().NotBeNull();
		collectors.Should().NotBeNullOrEmpty();
		var refetchedCollector = await LogicMonitorClient
			.GetAsync<Collector>(collectors[0].Id, CancellationToken);

		refetchedCollector.Should().NotBeNull();
	}

	[Fact]
	public async Task Get1Collector()
	{
		var collector = await LogicMonitorClient
			.GetAsync<Collector>(CollectorId, CancellationToken);

		collector.Should().NotBeNull();
	}

	[Fact]
	public async Task AcknowledgeCollectorDownAsync_CallWithValidCollectorId_DoesNotThrowException()
	{
		var collector = await LogicMonitorClient
			.GetAsync<Collector>(DownCollectorId, CancellationToken);

		collector.Should().NotBeNull();
		collector.Status.Should().Be(1);

		var action = async () => await LogicMonitorClient
			.AcknowledgeCollectorDownAsync(DownCollectorId, new() { Comment = "This collector exists solely for testing" }, CancellationToken);

		await action.Should().NotThrowAsync();
	}
}
