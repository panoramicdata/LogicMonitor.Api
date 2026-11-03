namespace LogicMonitor.Api.Test.Settings;

public class AccessGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllAsync_Succeeds()
	{
		var accessGroups = await LogicMonitorClient.GetAllAsync<AccessGroup>(CancellationToken);

		accessGroups.Should().NotBeNullOrEmpty();
		accessGroups.Should().AllSatisfy(g => g.Name.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task CreateGetDelete_Succeeds()
	{
		var name = $"LMApiTest {Guid.NewGuid():N}"[..18];
		var created = await LogicMonitorClient.CreateAsync(
			new AccessGroupCreationDto { Name = name, Description = "Created by LogicMonitor.Api.Test" },
			CancellationToken);
		try
		{
			created.Id.Should().BePositive();

			var fetched = await LogicMonitorClient.GetAsync<AccessGroup>(created.Id, CancellationToken);
			fetched.Name.Should().Be(name);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(created, CancellationToken);
		}
	}
}
