using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorTests : TestWithOutput
{
	public CollectorTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void ExecuteDebugCommand()
	{
		var debugCommandResponse = await LogicMonitorClient.ExecuteDebugCommandAsync(CollectorId, "!ping 8.8.8.8").ConfigureAwait(false);

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		Assert.True(debugCommandResponse.SessionId > 0);
	}

	[Fact]
	public async void ExecuteDebugCommandAndWaitForResult()
	{
		var debugCommandResponse = await LogicMonitorClient.ExecuteDebugCommandAndWaitForResultAsync(CollectorId, "!ping 8.8.8.8", 20000, 100).ConfigureAwait(false);

		// Check for valid response
		debugCommandResponse.Should().NotBeNull();
		debugCommandResponse.Output.Should().NotBeNullOrEmpty();
		Logger.LogInformation("{Message}", debugCommandResponse.Output);
	}

	[Fact]
	public async void GetCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);

		// Make sure that some are returned
		collectors.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		collectors.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Get each one by id
		foreach (var collector in collectors)
		{
			var refetchedCollector = await LogicMonitorClient.GetAsync<Collector>(collector.Id).ConfigureAwait(false);
			// TODO - make sure they match
		}
	}

	[Fact]
	public async void GetCollector()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		collectors.Should().NotBeNull();
		collectors.Should().NotBeNullOrEmpty();
		var refetchedCollector = await LogicMonitorClient
			.GetAsync<Collector>(collectors[0].Id)
			.ConfigureAwait(false);

		refetchedCollector.Should().NotBeNull();
	}

	[Fact]
	public async void Get1Collector()
	{
		var collector = await LogicMonitorClient
			.GetAsync<Collector>(CollectorId, default)
			.ConfigureAwait(false);

		collector.Should().NotBeNull();
	}
}
