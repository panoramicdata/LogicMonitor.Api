using Xunit;
using Xunit.Abstractions;

// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules
{
	public class EventSourceTests : TestWithOutput
	{
		public EventSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetDeviceGroupEventSources()
		{
			var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(deviceGroup);

			var eventGroupDataSources = await LogicMonitorClient.GetAllDeviceGroupEventSourcesAsync(deviceGroup.Id).ConfigureAwait(false);
			Assert.NotEmpty(eventGroupDataSources);
		}
	}
}