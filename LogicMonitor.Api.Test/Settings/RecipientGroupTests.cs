using LogicMonitor.Api.Settings;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings;

public class RecipientGroupTests : TestWithOutput
{
	public RecipientGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetRecipientGroupTests()
	{
		var recipientGroups = await LogicMonitorClient.GetAllAsync<RecipientGroup>().ConfigureAwait(false);
		Assert.NotNull(recipientGroups);
		Assert.True(recipientGroups.Count > 0);

		foreach (var recipientGroup in recipientGroups)
		{
			var refetchedRole = await LogicMonitorClient.GetAsync<RecipientGroup>(recipientGroup.Id).ConfigureAwait(false);
			Assert.True(refetchedRole.GroupName == recipientGroup.GroupName);
		}
	}
}
