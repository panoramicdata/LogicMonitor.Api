namespace LogicMonitor.Api.Test.LogicModules;

public class AppliesToFunctionTests : TestWithOutput
{
	public AppliesToFunctionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void CreateUpdateAndDelete()
	{
		const string testAppliesToFunctionName = "XxxUnitTest";
		const string testAppliesToFunctionDescription = testAppliesToFunctionName + " - Description";
		const string testAppliesToFunctionCode = "displayname == \"" + testAppliesToFunctionName + "\"";

		// Delete if already present
		var existingAppliesToFunction = await LogicMonitorClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
		if (existingAppliesToFunction is not null)
		{
			await LogicMonitorClient.DeleteAsync(existingAppliesToFunction).ConfigureAwait(false);
		}

		// Create
		var createdAppliesToFunction = await LogicMonitorClient.CreateAsync(new AppliesToFunctionCreationDto
		{
			Name = testAppliesToFunctionName,
			Description = testAppliesToFunctionDescription,
			Code = testAppliesToFunctionCode
		}).ConfigureAwait(false);

		// Refetch and check
		existingAppliesToFunction = await LogicMonitorClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
		existingAppliesToFunction.Should().NotBeNull();
		createdAppliesToFunction.Id.Should().Be(existingAppliesToFunction.Id);

		// Update
		const string newDescription = testAppliesToFunctionDescription + "2";
		existingAppliesToFunction.Description = newDescription;
		await LogicMonitorClient.PutAsync(existingAppliesToFunction).ConfigureAwait(false);

		// Refetch and check
		existingAppliesToFunction = await LogicMonitorClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
		existingAppliesToFunction.Description.Should().Be(newDescription);

		// Delete
		await LogicMonitorClient.DeleteAsync(existingAppliesToFunction).ConfigureAwait(false);

		// Refetch and check
		existingAppliesToFunction = await LogicMonitorClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
		existingAppliesToFunction.Should().BeNull();
	}

	[Theory]
	[InlineData("1.2.3/24", "join(system.ips, \",\") =~ \"(^|,)1\\\\.2\\\\.3\\\\.\\\\d+(,|$)\"")]
	public void AppliesToFromCidrTests_Succeeds(string input, string expected)
	{
		var appliesToFunction = new AppliesToFunction();
		appliesToFunction.SetCodeFromCidr(input);
		appliesToFunction.Code.Should().Be(expected);
	}

	[Theory]
	[InlineData("1.2.3")]
	[InlineData("1.2.3.4")]
	[InlineData("1.2.3.4/33")]
	[InlineData("1.2.3.4/x")]
	[InlineData("x/x")]
	[InlineData("1.2.333.0/24")]
	public void AppliesToFromCidrTests_ThrowsFormatException(string input)
	{
		var appliesToFunction = new AppliesToFunction();
		Assert.Throws<FormatException>(() => appliesToFunction.SetCodeFromCidr(input));
	}

	[Theory]
	[InlineData("0.0.0.0/0")]
	[InlineData("0.0.0.0/1")]
	[InlineData("0.0.0.0/2")]
	[InlineData("0.0.0.0/3")]
	[InlineData("0.0.0.0/4")]
	[InlineData("0.0.0.0/5")]
	[InlineData("0.0.0.0/6")]
	[InlineData("0.0.0.0/7")]
	[InlineData("0.0.0.0/8")]
	[InlineData("0.0.0.0/9")]
	[InlineData("0.0.0.0/10")]
	[InlineData("0.0.0.0/11")]
	[InlineData("0.0.0.0/12")]
	[InlineData("0.0.0.0/13")]
	[InlineData("0.0.0.0/14")]
	[InlineData("0.0.0.0/15")]
	[InlineData("0.0.0.0/16")]
	[InlineData("0.0.0.0/17")]
	[InlineData("0.0.0.0/18")]
	[InlineData("0.0.0.0/19")]
	[InlineData("0.0.0.0/20")]
	[InlineData("0.0.0.0/21")]
	[InlineData("0.0.0.0/22")]
	public void AppliesToFromCidrTests_ThrowsNotSupportedException(string input)
	{
		var appliesToFunction = new AppliesToFunction();
		Assert.Throws<NotSupportedException>(() => appliesToFunction.SetCodeFromCidr(input));
	}

	[Fact]
	public async void CustomerCodeWorks()
	{
		var matches = await LogicMonitorClient
			.GetAppliesToAsync("customer.code == \"PDL\"")
			.ConfigureAwait(false);
		matches.Should().NotBeNull();
		matches.Should().NotBeEmpty();
	}
}
