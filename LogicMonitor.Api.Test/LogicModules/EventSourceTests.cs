// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests : TestWithOutput
{
	public EventSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDeviceGroupEventSources()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();

		var eventGroupDataSources = await LogicMonitorClient
			.GetAllDeviceGroupEventSourcesAsync(deviceGroup.Id, default)
			.ConfigureAwait(false);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
