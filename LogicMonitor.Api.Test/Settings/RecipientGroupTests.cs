namespace LogicMonitor.Api.Test.Settings;

public class RecipientGroupTests : TestWithOutput
{
	public RecipientGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetRecipientGroupTests()
	{
		var recipientGroups = await LogicMonitorClient.GetAllAsync<RecipientGroup>().ConfigureAwait(false);
		recipientGroups.Should().NotBeNullOrEmpty();

		foreach (var recipientGroup in recipientGroups)
		{
			var refetchedRole = await LogicMonitorClient.GetAsync<RecipientGroup>(recipientGroup.Id).ConfigureAwait(false);
			refetchedRole.GroupName.Should().Be(recipientGroup.GroupName);
		}
	}
}
