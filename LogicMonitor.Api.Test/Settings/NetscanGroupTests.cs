namespace LogicMonitor.Api.Test.Settings;

public class NetscanGroupTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task CanCreateAndDeleteNetscanGroups()
	{
		const string name = "API Unit Test CanCreateAndDeleteNetscanGroups";
		const string description = "API Unit Test CanCreateAndDeleteNetscanGroups Description";

		var allNetscanGroups = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);

		var existingTestNetscanGroup = allNetscanGroups
			.SingleOrDefault(group => group.Name == name);
		if (existingTestNetscanGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync<NetscanGroup>(existingTestNetscanGroup.Id, cancellationToken: default)
				.ConfigureAwait(true);
		}

		var netscanGroups = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);
		netscanGroups.Should().AllSatisfy(group => group.Name.Should().NotBe(name));
		// Definitely not there now

		// Create one
		var netscanGroupCreationDto = new NetscanGroupCreationDto
		{
			Name = name,
			Description = description
		};
		var newNetscanGroup = await LogicMonitorClient
			.CreateAsync(netscanGroupCreationDto, default)
			.ConfigureAwait(true);
		var netscanGroupsRefetched = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);
		netscanGroupsRefetched.Should().Contain(group => group.Name == name);

		await LogicMonitorClient
			.DeleteAsync(newNetscanGroup, cancellationToken: default)
			.ConfigureAwait(true);
		netscanGroupsRefetched = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);
		netscanGroupsRefetched.Should().NotContain(group => group.Name == name);
	}

	[Fact]
	public async Task CanGetNetscanGroups()
	{
		var allNetscanGroups = await LogicMonitorClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);
		allNetscanGroups.Should().NotBeNull();
		allNetscanGroups.Should().NotBeNullOrEmpty();
		var ids = allNetscanGroups.Select(nspg => nspg.Id);
		ids.Should().HaveCount(allNetscanGroups.Count);
	}
}
