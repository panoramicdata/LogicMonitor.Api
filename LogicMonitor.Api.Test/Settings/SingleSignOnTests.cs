using LogicMonitor.Api.Settings;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class SingleSignOnTests : TestWithOutput
	{
		public SingleSignOnTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetSingleSignOnData()
		{
			var allOpsNotes = await PortalClient.GetAsync<SingleSignOn>().ConfigureAwait(false);

			// Text should be set
			Assert.NotEmpty(allOpsNotes.SamlVersion);
		}
	}
}