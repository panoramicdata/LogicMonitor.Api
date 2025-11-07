namespace LogicMonitor.Api.Test.Settings;

public class RecipientGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetRecipientGroupTests()
	{
		var recipientGroups = await LogicMonitorClient
			.GetAllAsync<RecipientGroup>(CancellationToken);
		recipientGroups.Should().NotBeNullOrEmpty();

		foreach (var recipientGroup in recipientGroups)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<RecipientGroup>(recipientGroup.Id, CancellationToken);
			refetchedRole.Name.Should().Be(recipientGroup.Name);
		}
	}
}
