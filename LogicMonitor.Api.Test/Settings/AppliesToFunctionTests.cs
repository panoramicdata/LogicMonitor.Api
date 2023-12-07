namespace LogicMonitor.Api.Test.Settings;

public class AppliesToFunctionTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAppliesToFunctionListSucceeds()
	{
		var appliesToFunctions = await LogicMonitorClient
			.GetAppliesToFunctionListAsync(new Filter<AppliesToFunction>(), default)
			.ConfigureAwait(true);
		appliesToFunctions.Should().NotBeNull();
		appliesToFunctions.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetSpecificAppliesToFunctionSucceeds()
	{
		var appliesToFunction = await LogicMonitorClient
			.GetAppliesToFunctionAsync(1, default)
			.ConfigureAwait(true);
		appliesToFunction.Name.Should().Be("NetSNMPComputers");
	}

	[Fact]
	public async Task AddAppliesToFunction()
	{
		var newATF = new AppliesToFunctionCreationDto()
		{
			Name = "Test",
			Description = "LornaTest",
			Code = "string"
		};
		await LogicMonitorClient
			.AddAppliesToFunctionAsync(newATF, default)
			.ConfigureAwait(true);
		var appliesToFunctions = await LogicMonitorClient
			.GetAppliesToFunctionListAsync(new Filter<AppliesToFunction>(), default)
			.ConfigureAwait(true);

		var found = false;
		var foundATF = new AppliesToFunction();
		foreach (AppliesToFunction atf in appliesToFunctions.Items)
		{
			if (atf.Name.Equals("Test", StringComparison.Ordinal))
			{
				found = true;
				foundATF = atf;
			}
		}

		if (found)
		{
			await LogicMonitorClient
				.DeleteAsync($"setting/functions/{foundATF.Id}", default)
				.ConfigureAwait(true);
		}

		found.Should().BeTrue();
		foundATF.Description.Should().Be("LornaTest");
	}
}
