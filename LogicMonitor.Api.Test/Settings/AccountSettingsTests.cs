using LogicMonitor.Api.Settings;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class AccountSettingsTests : TestWithOutput
	{
		public AccountSettingsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void Get()
		{
			var accountSettings = await PortalClient.GetAsync<AccountSettings>().ConfigureAwait(false);
			Assert.NotNull(accountSettings);
			Assert.True(accountSettings.DeviceCount > 0);
		}

		[Fact]
		public async void GetBillingInformation()
		{
			if (!AccountHasBillingInformation)
			{
				// Our test account does not have billing information - we can't test this.
				return;
			}

			var billingInformation = await PortalClient.GetAsync<BillingInformation>().ConfigureAwait(false);

			Assert.NotNull(billingInformation);
			Assert.NotNull(billingInformation.InvoiceDetails);
			Assert.NotNull(billingInformation.PaymentInformation);
		}
	}
}