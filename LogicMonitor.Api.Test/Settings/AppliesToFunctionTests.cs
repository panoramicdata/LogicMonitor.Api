namespace LogicMonitor.Api.Test.Settings;

public class AppliesToFunctionTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAppliesToFunctionListSucceeds()
	{
		var appliesToFunctions = await LogicMonitorClient
			.GetAppliesToFunctionListAsync(new Filter<AppliesToFunction>(), CancellationToken);
		appliesToFunctions.Should().NotBeNull();
		appliesToFunctions.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetSpecificAppliesToFunctionSucceeds()
	{
		var appliesToFunction = await LogicMonitorClient
			.GetAppliesToFunctionAsync(1, CancellationToken);
		appliesToFunction.Name.Should().Be("NetSNMPComputers");
	}

	[Fact]
	public async Task AddAppliesToFunction()
	{
		const string TestAppliesToFunctionDescription = "LornaTest";
		const string TestAppliesToFunctionName = "Test";

		var filter = new Filter<AppliesToFunction>
		{
			FilterItems = [
				new Eq<AppliesToFunction>(nameof(AppliesToFunction.Name), TestAppliesToFunctionName)
			]
		};

		// Delete the test AppliesToFunction if it already exists
		var appliesToFunctions = await LogicMonitorClient
			.GetAppliesToFunctionListAsync(filter, CancellationToken);

		foreach (var atf in appliesToFunctions.Items)
		{
			await LogicMonitorClient
				.DeleteAsync<AppliesToFunction>(atf.Id, CancellationToken);
		}

		var newATF = new AppliesToFunctionCreationDto()
		{
			Name = TestAppliesToFunctionName,
			Description = TestAppliesToFunctionDescription,
			Code = "string"
		};

		await LogicMonitorClient
			.AddAppliesToFunctionAsync(newATF, CancellationToken);

		appliesToFunctions = await LogicMonitorClient
			.GetAppliesToFunctionListAsync(new Filter<AppliesToFunction>(), CancellationToken);

		var foundATF = appliesToFunctions.Items.SingleOrDefault(atf => atf.Name == TestAppliesToFunctionName);
		foundATF.Should().NotBeNull();
		foundATF.Name.Should().Be(TestAppliesToFunctionName);
		foundATF.Description.Should().Be(TestAppliesToFunctionDescription);

		await LogicMonitorClient
				.DeleteAsync<AppliesToFunction>(foundATF.Id, CancellationToken);
	}
}
