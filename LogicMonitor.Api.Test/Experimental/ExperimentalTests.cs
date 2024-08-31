using LogicMonitor.Api.Experimental;

namespace LogicMonitor.Api.Test.Experimental;

public class ExperimentalTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDevices_Succeeds()
	{
		var devices = await ExperimentalLogicMonitorClient
			.GetAsync(new LogicMonitorRequest<Device>
			{
				Skip = 1,
				Take = 1,
				Properties = [nameof(Device.Id), nameof(Device.Name)],
				Filter = new AdvancedFilter<Device>(
					[
						new FilterItem<Device>
						{
							Property = nameof(Device.DisplayName),
							Comparator = Comparator.Includes,
							Value = "test"
						}
					]
				)
			}, default)
			.ConfigureAwait(true);

		devices.Should().NotBeNullOrEmpty();
		devices.Should().ContainSingle();
		devices.First().Name.Should().NotBeNullOrWhiteSpace();
		devices.First().Description.Should().BeNullOrEmpty();
		devices.First().Id.Should().BePositive();
	}
}
