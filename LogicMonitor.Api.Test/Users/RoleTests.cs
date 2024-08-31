namespace LogicMonitor.Api.Test.Users;

public class RoleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<Role>(default)
			.ConfigureAwait(true);

		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}
}
