namespace LogicMonitor.Api.Test.Settings;

public class NetscanGroupTests : TestWithOutput
{
	public NetscanGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void CanCreateAndDeleteNetscanGroups()
	{
		var allNetscanGroups = await LogicMonitorClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
		const string name = "API Unit Test CanCreateAndDeleteNetscanGroups";
		const string description = "API Unit Test CanCreateAndDeleteNetscanGroups Description";

		var existingTestNetscanGroup = allNetscanGroups.SingleOrDefault(group => group.Name == name);
		if (existingTestNetscanGroup != null)
		{
			await LogicMonitorClient.DeleteAsync<NetscanGroup>(existingTestNetscanGroup.Id).ConfigureAwait(false);
		}
		Assert.DoesNotContain(await LogicMonitorClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);
		// Definitely not there now

		// Create one
		var netscanGroupCreationDto = new NetscanGroupCreationDto
		{
			Name = name,
			Description = description
		};
		var newNetscanGroup = await LogicMonitorClient.CreateAsync(netscanGroupCreationDto).ConfigureAwait(false);
		Assert.Contains(await LogicMonitorClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);

		await LogicMonitorClient.DeleteAsync(newNetscanGroup).ConfigureAwait(false);
		Assert.DoesNotContain(await LogicMonitorClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);
	}

	[Fact]
	public async void CanGetNetscanGroups()
	{
		var allNetscanGroups = await LogicMonitorClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
		Assert.NotNull(allNetscanGroups);
		Assert.NotEmpty(allNetscanGroups);
		var ids = allNetscanGroups.Select(nspg => nspg.Id);
		Assert.True(allNetscanGroups.Count == ids.Count());
		Assert.NotEmpty(allNetscanGroups);
	}
}
