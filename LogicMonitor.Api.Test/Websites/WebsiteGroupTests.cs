namespace LogicMonitor.Api.Test.Websites;

public class WebsiteGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task SetWebsiteGroupCustomProperty()
	{
		// Create a ResourceGroup for testing purposes
		const string testWebsiteGroupName = "Property Test ResourceGroup";
		var existingWebsiteGroup = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(testWebsiteGroupName, default)
			.ConfigureAwait(true);
		if (existingWebsiteGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingWebsiteGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}

		var websiteGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
		{
			ParentId = "1",
			Name = testWebsiteGroupName
		}, default).ConfigureAwait(true);

		const string propertyName = "blah";
		const string value1 = "test1";
		const string value2 = "test2";

		// Set it to an expected value
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, value1)
			.ConfigureAwait(true);
		var deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, value2)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, null)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);

		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(true)).ConfigureAwait(true);
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(true)).ConfigureAwait(true);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(true)).ConfigureAwait(true);
		createNullException.Should().BeOfType<InvalidOperationException>();

		// Create one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, value1, SetPropertyMode.Create)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, value2, SetPropertyMode.Update)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetWebsiteGroupCustomPropertyAsync(websiteGroup.Id, propertyName, null, SetPropertyMode.Delete)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetWebsiteGroupPropertiesAsync(websiteGroup.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
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
			}, default)
			.ConfigureAwait(true);

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
				.DeleteAsync(websiteGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}
	}

	[Fact]
	public async Task GetWebsiteGroupByFullPath()
	{
		var websiteGroup0 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(string.Empty, default)
			.ConfigureAwait(true);
		websiteGroup0.Should().NotBeNull();
		websiteGroup0 ??= new();
		websiteGroup0.FullPath.Should().Be(string.Empty);
		var websiteGroup1 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath, default)
			.ConfigureAwait(true);
		websiteGroup1.Should().NotBeNull();
		var websiteGroup2 = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath, default)
			.ConfigureAwait(true);
		websiteGroup2.Should().NotBeNull();
		websiteGroup2 ??= new();
		websiteGroup0.Id.Should().NotBe(websiteGroup2.Id);
	}

	[Fact]
	public async Task GetWebsiteGroupById()
	{
		var websiteGroup = await LogicMonitorClient.GetAsync<WebsiteGroup>(1, default).ConfigureAwait(true);

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
			.GetWebsitesByWebsiteGroupIdAsync(1, new Filter<Website>(), default)
			.ConfigureAwait(true);

		websites.Items.Should().NotBeNull();
	}
}
