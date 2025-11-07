namespace LogicMonitor.Api.Test.Users;

public class UserTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAll()
	{
		var users = await LogicMonitorClient.GetAllAsync<User>(CancellationToken);

		users.Should().NotBeNull();
		users.Should().NotBeNullOrEmpty();
	}
}
