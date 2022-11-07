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
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken.None)
			.ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();

		var eventGroupDataSources = await LogicMonitorClient
			.GetAllDeviceGroupEventSourcesAsync(deviceGroup.Id, CancellationToken.None)
			.ConfigureAwait(false);
		eventGroupDataSources.Should().NotBeNullOrEmpty();
	}
}
