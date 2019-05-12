using LogicMonitor.Api.Settings;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class IntegrationTests : TestWithOutput
	{
		public IntegrationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetIntegrations()
		{
			var integrations = await PortalClient.GetAllAsync<Integration>().ConfigureAwait(false);

			// Text should be set
			Assert.All(integrations, on => Assert.False(string.IsNullOrWhiteSpace(on.Name)));
		}
	}
}