namespace LogicMonitor.Api.Test.Settings;

public class UsageTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetContractInfo_Succeeds()
	{
		var contractInfo = await LogicMonitorClient.GetContractInfoAsync(CancellationToken);

		contractInfo.Should().NotBeEmpty();
		contractInfo.Should().AllSatisfy(c => c.Product.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task GetMetricsSummary_Succeeds()
	{
		var summary = await LogicMonitorClient.GetMetricsSummaryAsync(CancellationToken);

		summary.NumberOfDevices.Should().BePositive();
	}

	[Fact]
	public async Task GetCloudRecommendationCategories_Succeeds()
	{
		var page = await LogicMonitorClient.GetCloudRecommendationCategoriesAsync(CancellationToken);

		page.Items.Should().NotBeEmpty();
		page.Items.Should().AllSatisfy(c => c.Name.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task GetAllCloudRecommendations_Succeeds()
	{
		var recommendations = await LogicMonitorClient.GetAllAsync<CloudRecommendation>(CancellationToken);

		recommendations.Should().NotBeNull();
	}

	[Fact]
	public async Task GetConfigSourceUpdateReasons_Succeeds()
	{
		var configSources = await LogicMonitorClient.GetPageAsync(new Filter<ConfigSource> { Take = 1 }, CancellationToken);
		configSources.Items.Should().NotBeEmpty();

		var page = await LogicMonitorClient.GetConfigSourceUpdateReasonsAsync(configSources.Items[0].Id, null, CancellationToken);

		page.Items.Should().NotBeNull();
	}
}
