namespace LogicMonitor.Api.Test.Settings;

public class NetscanGroupTests : TestWithOutput
{
	public NetscanGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task CanCreateAndDeleteNetscanGroups()
	{
		const string name = "API Unit Test CanCreateAndDeleteNetscanGroups";
		const string description = "API Unit Test CanCreateAndDeleteNetscanGroups Description";

		var allNetscanGroups = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(CancellationToken.None)
			.ConfigureAwait(false);

		var existingTestNetscanGroup = allNetscanGroups
			.SingleOrDefault(group => group.Name == name);
		if (existingTestNetscanGroup is not null)
		{
			await LogicMonitorClient.DeleteAsync<NetscanGroup>(existingTestNetscanGroup.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		}

		var netscanGroups = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(CancellationToken.None)
			.ConfigureAwait(false);
		netscanGroups.Should().AllSatisfy(group => group.Name.Should().NotBe(name));
		// Definitely not there now

		// Create one
		var netscanGroupCreationDto = new NetscanGroupCreationDto
		{
			Name = name,
			Description = description
		};
		var newNetscanGroup = await LogicMonitorClient.CreateAsync(netscanGroupCreationDto, CancellationToken.None).ConfigureAwait(false);
		var netscanGroupsRefetched = await LogicMonitorClient.GetAllAsync<NetscanGroup>(CancellationToken.None).ConfigureAwait(false);
		netscanGroupsRefetched.Should().Contain(group => group.Name == name);

		await LogicMonitorClient.DeleteAsync(newNetscanGroup, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		netscanGroupsRefetched = await LogicMonitorClient.GetAllAsync<NetscanGroup>(CancellationToken.None).ConfigureAwait(false);
		netscanGroupsRefetched.Should().NotContain(group => group.Name == name);
	}

	[Fact]
	public async Task CanGetNetscanGroups()
	{
		var allNetscanGroups = await LogicMonitorClient.GetAllAsync<NetscanGroup>(CancellationToken.None).ConfigureAwait(false);
		allNetscanGroups.Should().NotBeNull();
		allNetscanGroups.Should().NotBeNullOrEmpty();
		var ids = allNetscanGroups.Select(nspg => nspg.Id);
		ids.Should().HaveCount(allNetscanGroups.Count);
	}
}
