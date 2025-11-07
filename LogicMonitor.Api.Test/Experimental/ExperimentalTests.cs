using LogicMonitor.Api.Experimental;

namespace LogicMonitor.Api.Test.Experimental;

public class ExperimentalTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetResources_Succeeds()
	{
		var devices = await ExperimentalLogicMonitorClient
			.GetAsync(new LogicMonitorRequest<Resource>
			{
				Skip = 1,
				Take = 1,
				Properties = [nameof(Resource.Id), nameof(Resource.Name)],
				Filter = new AdvancedFilter<Resource>(
					[
						new FilterItem<Resource>
						{
							Property = nameof(Resource.DisplayName),
							Comparator = Comparator.Includes,
							Value = "test"
						}
					]
				)
			}, CancellationToken);

		devices.Should().NotBeNullOrEmpty();
		devices.Should().ContainSingle();
		devices.First().Name.Should().NotBeNullOrWhiteSpace();
		devices.First().Description.Should().BeNullOrEmpty();
		devices.First().Id.Should().BePositive();
	}
}
