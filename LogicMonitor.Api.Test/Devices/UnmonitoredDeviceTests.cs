using LogicMonitor.Api.Devices;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Devices
{
	public class UnmonitoredDeviceTests : TestWithOutput
	{
		public UnmonitoredDeviceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetUnmonitoredDevices()
		{
			var unmonitoredDevices = await LogicMonitorClient.GetAllAsync<UnmonitoredDevice>().ConfigureAwait(false);
			Assert.NotNull(unmonitoredDevices);
		}
	}
}