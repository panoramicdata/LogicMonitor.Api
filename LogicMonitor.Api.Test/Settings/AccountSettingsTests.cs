namespace LogicMonitor.Api.Test.Settings;

public class AccountSettingsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task Get()
	{
		var accountSettings = await LogicMonitorClient
			.GetAsync<AccountSettings>(CancellationToken);
		accountSettings.Should().NotBeNull();
		(accountSettings.ResourceCount > 0).Should().BeTrue();
	}

	[Fact]
	public async Task GetBillingInformation()
	{
		if (!AccountHasBillingInformation)
		{
			// Our test account does not have billing information - we can't test this.
			return;
		}

		var billingInformation = await LogicMonitorClient
			.GetAsync<BillingInformation>(CancellationToken);

		billingInformation.Should().NotBeNull();
	}

	[Fact]
	public async Task GetMetrics()
	{
		var metrics = await LogicMonitorClient
			.GetMetricsAsync(CancellationToken);
		metrics.Should().NotBeNull();
	}
}
