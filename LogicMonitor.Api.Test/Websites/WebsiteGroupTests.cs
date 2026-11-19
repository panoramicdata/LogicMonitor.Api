namespace LogicMonitor.Api.Test.Websites;

public class WebsiteGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string CustomPropertyName = "blah";
	private const string CustomPropertyValue1 = "test1";
	private const string CustomPropertyValue2 = "test2";

	[Fact]
	public async Task SetWebsiteGroupCustomProperty()
	{
		// Create a ResourceGroup for testing purposes
		const string testWebsiteGroupName = "Property Test ResourceGroup";
		var existingWebsiteGroup = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(testWebsiteGroupName, CancellationToken);
		if (existingWebsiteGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingWebsiteGroup, CancellationToken);
		}

		var websiteGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
		{
			ParentId = "1",
			Name = testWebsiteGroupName
		}, CancellationToken);

		await SetAndVerifyWebsiteGroupCustomPropertyValuesAsync(websiteGroup);
		await AssertInvalidWebsiteGroupCustomPropertyWritesThrowAsync(websiteGroup);
		await CreateUpdateAndDeleteWebsiteGroupCustomPropertyWithoutCheckingAsync(websiteGroup);
	}

	private async Task SetAndVerifyWebsiteGroupCustomPropertyValuesAsync(WebsiteGroup websiteGroup)
	{
		// Set it to an expected value
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, CustomPropertyValue1, cancellationToken: CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		var actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, CustomPropertyValue2, cancellationToken: CancellationToken);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, null, cancellationToken: CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	private async Task AssertInvalidWebsiteGroupCustomPropertyWritesThrowAsync(WebsiteGroup websiteGroup)
	{
		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, null, SetPropertyMode.Delete, CancellationToken));
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, null, SetPropertyMode.Update, CancellationToken));
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, null, SetPropertyMode.Create, CancellationToken));
		createNullException.Should().BeOfType<InvalidOperationException>();
	}

	private async Task CreateUpdateAndDeleteWebsiteGroupCustomPropertyWithoutCheckingAsync(WebsiteGroup websiteGroup)
	{
		// Create one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, CustomPropertyValue1, SetPropertyMode.Create, CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		var actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, CustomPropertyValue2, SetPropertyMode.Update, CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, CustomPropertyName, null, SetPropertyMode.Delete, CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task WebsiteGroupCreationShouldSetProperties()
	{
		// Create it
		var websiteGroup = await LogicMonitorClient
			.CreateAsync(new WebsiteGroupCreationDto
			{
				Name = "Test Name",
				Description = "Test Description",
				IsAlertingDisabled = false,
				IsMonitoringDisabled = false,
				ParentGroupFullPath = "",
				ParentId = "1",
				Properties = [new EntityProperty { Name = "name", Value = "value" }],
			}, CancellationToken);

		try
		{
			// Check everything was set correctly
			websiteGroup.Name.Should().Be("Test Name");
			websiteGroup.Description.Should().Be("Test Description");
			websiteGroup.DisableAlerting.Should().BeFalse();
			websiteGroup.StopMonitoring.Should().BeFalse();
			websiteGroup.FullPath.Should().Be("Test Name");
			websiteGroup.ParentId.Should().Be(1);
			websiteGroup.CustomProperties.Should().BeEquivalentTo(new List<EntityProperty> { new() { Name = "name", Value = "value" } });
		}
		finally
		{
			// Delete it
			await LogicMonitorClient
				.DeleteAsync(websiteGroup, CancellationToken);
		}
	}

	[Fact]
	public async Task GetWebsiteGroupByFullPath()
	{
		var websiteGroup0 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(string.Empty, CancellationToken);
		websiteGroup0.Should().NotBeNull();
		websiteGroup0 ??= new();
		websiteGroup0.FullPath.Should().Be(string.Empty);
		var websiteGroup1 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath, CancellationToken);
		websiteGroup1.Should().NotBeNull();
		var websiteGroup2 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath, CancellationToken);
		websiteGroup2.Should().NotBeNull();
		websiteGroup2 ??= new();
		websiteGroup0.Id.Should().NotBe(websiteGroup2.Id);
	}

	[Fact]
	public async Task GetWebsiteGroupById()
	{
		var websiteGroup = await LogicMonitorClient.GetAsync<WebsiteGroup>(1, CancellationToken);

		websiteGroup.Should().NotBeNull();
		websiteGroup.ChildWebsiteGroups.Should().NotBeNullOrEmpty();
		websiteGroup.ParentId.Should().Be(0);
		websiteGroup.Id.Should().Be(1);
		websiteGroup.Name.Should().NotBeNullOrWhiteSpace();
		(websiteGroup.FullPath is null).Should().BeFalse();
	}

	[Fact]
	public async Task GetWebsitesInGroup()
	{
		var websites = await LogicMonitorClient
			.GetWebsitesByWebsiteGroupIdAsync(1, new Filter<Website>(), CancellationToken);

		websites.Items.Should().NotBeNull();
	}
}
