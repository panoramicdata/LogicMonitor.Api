namespace LogicMonitor.Api.Test.Users;

public class UserTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAll()
	{
		var users = await LogicMonitorClient.GetAllAsync<User>(default).ConfigureAwait(true);

		users.Should().NotBeNull();
		users.Should().NotBeNullOrEmpty();
	}
}
