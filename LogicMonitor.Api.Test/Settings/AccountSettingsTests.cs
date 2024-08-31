namespace LogicMonitor.Api.Test.Settings;

public class AccountSettingsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task Get()
	{
		var accountSettings = await LogicMonitorClient
			.GetAsync<AccountSettings>(default)
			.ConfigureAwait(true);
		accountSettings.Should().NotBeNull();
		(accountSettings.DeviceCount > 0).Should().BeTrue();
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
			.GetAsync<BillingInformation>(default)
			.ConfigureAwait(true);

		billingInformation.Should().NotBeNull();
	}

	[Fact]
	public async Task GetMetrics()
	{
		var metrics = await LogicMonitorClient
			.GetMetricsAsync(default)
			.ConfigureAwait(true);
		metrics.Should().NotBeNull();
	}
}
