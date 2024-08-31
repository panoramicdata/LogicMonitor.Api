namespace LogicMonitor.Api.Test.Settings;

public class RecipientGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetRecipientGroupTests()
	{
		var recipientGroups = await LogicMonitorClient
			.GetAllAsync<RecipientGroup>(default)
			.ConfigureAwait(true);
		recipientGroups.Should().NotBeNullOrEmpty();

		foreach (var recipientGroup in recipientGroups)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<RecipientGroup>(recipientGroup.Id, default)
				.ConfigureAwait(true);
			refetchedRole.Name.Should().Be(recipientGroup.Name);
		}
	}
}
