namespace LogicMonitor.Api.Test.Resources;

public class ResourceDeltaTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task StartAndGetResourceDelta_Succeeds()
	{
		var started = await LogicMonitorClient.StartResourceDeltaAsync(new Filter<Resource> { Take = 5 }, CancellationToken);

		started.DeltaId.Should().NotBeNullOrWhiteSpace();
		started.Items.Should().NotBeEmpty();

		var delta = await LogicMonitorClient.GetResourceDeltaAsync(started.DeltaId, CancellationToken);
		delta.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetResourceSdts_Succeeds()
	{
		var resources = await LogicMonitorClient.GetPageAsync(new Filter<Resource> { Take = 1 }, CancellationToken);
		resources.Items.Should().NotBeEmpty();

		var page = await LogicMonitorClient.GetResourceSdtsAsync(resources.Items[0].Id, null, CancellationToken);

		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAwsAccountId_Succeeds()
	{
		var awsAccountId = await LogicMonitorClient.GetAwsAccountIdAsync(CancellationToken);

		awsAccountId.AccountId.Should().NotBeNullOrWhiteSpace();
	}
}
