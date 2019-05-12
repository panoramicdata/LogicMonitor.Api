using Xunit;
using Xunit.Abstractions;

// Older, now deprecated methods are still tested here
#pragma warning disable 618

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
			var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(deviceGroup);

			var eventGroupDataSources = await PortalClient.GetAllDeviceGroupEventSourcesAsync(deviceGroup.Id).ConfigureAwait(false);
			Assert.NotEmpty(eventGroupDataSources);
		}
	}
}

#pragma warning restore 618