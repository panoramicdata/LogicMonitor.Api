// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests : TestWithOutput
{
	public EventSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetDeviceGroupEventSources()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();

		var eventGroupDataSources = await LogicMonitorClient.GetAllDeviceGroupEventSourcesAsync(deviceGroup.Id).ConfigureAwait(false);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
