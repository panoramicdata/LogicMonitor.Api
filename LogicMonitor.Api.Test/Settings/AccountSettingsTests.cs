namespace LogicMonitor.Api.Test.Settings;

public class AccountSettingsTests : TestWithOutput
{
	public AccountSettingsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void Get()
	{
		var accountSettings = await LogicMonitorClient.GetAsync<AccountSettings>().ConfigureAwait(false);
		accountSettings.Should().NotBeNull();
		(accountSettings.DeviceCount > 0).Should().BeTrue();
	}

	[Fact]
	public async void GetBillingInformation()
	{
		if (!AccountHasBillingInformation)
		{
			// Our test account does not have billing information - we can't test this.
			return;
		}

		var billingInformation = await LogicMonitorClient.GetAsync<BillingInformation>().ConfigureAwait(false);

		billingInformation.Should().NotBeNull();
		billingInformation.InvoiceDetails.Should().NotBeNull();
		billingInformation.PaymentInformation.Should().NotBeNull();
	}
}
