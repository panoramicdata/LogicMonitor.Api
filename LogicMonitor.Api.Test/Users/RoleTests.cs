namespace LogicMonitor.Api.Test.Users;

public class RoleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<Role>(CancellationToken);

		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}
}
