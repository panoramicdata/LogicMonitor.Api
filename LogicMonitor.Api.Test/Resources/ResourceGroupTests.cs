using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Resources;

public partial class ResourceGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string CustomPropertyName = "blah";
	private const string CustomPropertyValue1 = "test1";
	private const string CustomPropertyValue2 = "test2";

	[Fact]
	public async Task SetResourceGroupCustomProperty()
	{
		// Make sure a ResourceGroup exists for testing purposes
		const string testResourceGroupName = "Property Test ResourceGroup";
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(
				testResourceGroupName,
				CancellationToken);

		resourceGroup ??= await LogicMonitorClient.CreateAsync(
					  new ResourceGroupCreationDto
					  {
						  ParentId = "1",
						  Name = testResourceGroupName
					  }, CancellationToken);

		try
		{
			await SetAndVerifyResourceGroupCustomPropertyValuesAsync(resourceGroup);
			await AssertInvalidResourceGroupCustomPropertyWritesThrowAsync(resourceGroup);
			await CreateUpdateAndDeleteResourceGroupCustomPropertyWithoutCheckingAsync(resourceGroup);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(resourceGroup, true, CancellationToken);
		}
	}

	private async Task SetAndVerifyResourceGroupCustomPropertyValuesAsync(ResourceGroup resourceGroup)
	{
		// Set it to an expected value
		await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(
			resourceGroup.Id,
			CustomPropertyName,
			CustomPropertyValue1,
			SetPropertyMode.Automatic,
			CancellationToken);
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		var actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(
			resourceGroup.Id,
			CustomPropertyName,
			CustomPropertyValue2,
			SetPropertyMode.Automatic,
			CancellationToken);
		resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Delete it
		await LogicMonitorClient
			.DeleteResourceGroupPropertyAsync(resourceGroup.Id, CustomPropertyName, CancellationToken);

		resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	private async Task AssertInvalidResourceGroupCustomPropertyWritesThrowAsync(ResourceGroup resourceGroup)
	{
		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, null, SetPropertyMode.Delete, CancellationToken));
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, null, SetPropertyMode.Update, CancellationToken));
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, null, SetPropertyMode.Create, CancellationToken));
		createNullException.Should().BeOfType<InvalidOperationException>();
	}

	private async Task CreateUpdateAndDeleteResourceGroupCustomPropertyWithoutCheckingAsync(ResourceGroup resourceGroup)
	{
		// Create one without checking
		await LogicMonitorClient
			.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, CustomPropertyValue1, SetPropertyMode.Create, CancellationToken);
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		var actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, CustomPropertyValue2, SetPropertyMode.Update, CancellationToken);
		resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, CustomPropertyName, null, SetPropertyMode.Delete, CancellationToken);
		resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
		actual = resourceGroupProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task GetResourceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(ResourceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetResourcesByResourceGroupFullPath_WithPropertiesFilter_FetchesOnlyThoseFields()
	{
		// Read-only. Asks for two fields only.
		var filtered = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(
				ResourceGroupFullPath,
				true,
				CancellationToken,
				new Filter<Resource>
				{
					Properties = [nameof(Resource.Id), nameof(Resource.ResourceType)]
				});

		filtered.Should().NotBeEmpty();

		// The requested field is populated...
		filtered.Should().AllSatisfy(resource => resource.Id.Should().BePositive());

		// ...and the ones not requested are not, which is only true if the filter
		// reached the API as fields=. If the parameter were accepted and ignored,
		// these would be populated and this test would fail.
		filtered.Should().AllSatisfy(resource => resource.Name.Should().BeEmpty());
		filtered.Should().AllSatisfy(resource => resource.DisplayName.Should().BeEmpty());

		// The same call with no filter returns the same Resources, fully populated,
		// so the narrowing above is caused by the filter and not by the group contents.
		var unfiltered = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(ResourceGroupFullPath, true, CancellationToken);

		filtered.Select(resource => resource.Id)
			.Should().BeEquivalentTo(unfiltered.Select(resource => resource.Id));
		// At least one, rather than all: across a whole estate a single oddly-named
		// Resource would otherwise fail this for reasons unrelated to the filter.
		unfiltered.Should().Contain(resource => !string.IsNullOrEmpty(resource.Name));
	}

	[Fact]
	public async Task GetResourcesByResourceGroupByFullPathWhereNameContainsParentheses()
	{
		// No recurse
		var nonrecurse = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", false, CancellationToken);

		// Recurse
		var recursed = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", true, CancellationToken);

		nonrecurse.Should().ContainSingle();
		recursed.Should().HaveCount(2);
	}

	[Fact]
	public async Task SetAlertThreshold()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(ResourceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", CancellationToken);
		dataSource ??= new();
		var datapoints = (await LogicMonitorClient
			.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataPoint> { Skip = 0, Take = 10 }, CancellationToken)
			).Items;
		var datapoint = datapoints.Single(dp => dp.Name == "average");
		await LogicMonitorClient
			.SetResourceGroupDataSourceDataPointThresholdsAsync(
				resourceGroup.Id,
				dataSource.Id,
				datapoint.Id,
				"> 191 timezone=Europe/London",
				$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
				true,
				CancellationToken);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetResourceGroupByFullPath_InvalidResourceGroup_ReturnsNull()
	{
		var nonExistentResourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("XXXXXX/YYYYYY", CancellationToken);
		nonExistentResourceGroup.Should().BeNull();
	}

	[Fact]
	public async Task GetResourceGroupById()
	{
		var rootResourceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, CancellationToken);
		rootResourceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRootResourceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("", CancellationToken) ?? throw new InvalidOperationException();
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetAllResourceGroups()
	{
		// For LMREP-4060, to test if there are any missing ResourceGroupTypes
		var resourceGroups = await LogicMonitorClient
			.GetAllAsync<ResourceGroup>(CancellationToken);
		resourceGroups.Should().NotBeNull();
		resourceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		resourceGroups.Select(dg => dg.Id).HasDuplicates().Should().BeFalse();

		// Make sure that all have non-zero Parent Ids
		resourceGroups.Should().AllSatisfy(dg => (dg.Id == 1 || dg.ParentId != 0).Should().BeTrue());
	}

	[Fact]
	public async Task GetResourceGroupProperties()
	{
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(1, CancellationToken);

		// Make sure that some are returned
		resourceGroupProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetResourceGroupPropertiesByName()
	{
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesByNameAsync(1, "api.account", CancellationToken);
		resourceGroupProperties.Name.Should().NotBe(string.Empty);
	}

	[Fact]
	public async Task GetResourceGroupsByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(ResourceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();

		var resourceGroups = resourceGroup.SubGroups;

		// Make sure that some are returned
		resourceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		resourceGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetOneResourceGroup_Succeeds()
	{
		var resourceGroup = await LogicMonitorClient
			.GetAllAsync(new Filter<ResourceGroup> { Take = 1 }, CancellationToken);

		// Make sure that some are returned
		resourceGroup.Should().ContainSingle();
	}
}
