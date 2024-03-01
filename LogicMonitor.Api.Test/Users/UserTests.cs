namespace LogicMonitor.Api.Test.Users;

public class UserTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAll()
	{
		var users = await LogicMonitorClient.GetAllAsync<User>(default).ConfigureAwait(true);

		users.Should().NotBeNull();
		users.Should().NotBeNullOrEmpty();
	}
}
