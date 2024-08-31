namespace LogicMonitor.Api.Test.Devices;

public class UnmonitoredDeviceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetUnmonitoredDevices()
	{
		var unmonitoredDevices = await LogicMonitorClient
			.GetUnmonitoredDeviceAsync(new Filter<UnmonitoredDevice>(), default)
			.ConfigureAwait(true);
		unmonitoredDevices.Should().NotBeNull();
	}
}
