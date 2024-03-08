using LogicMonitor.Api.Logging;

namespace LogicMonitor.Api.Test.Logging;

public class LoggingTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task WriteLogAsync_WithResourceId_Succeeds()
	{
		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, WindowsDeviceId, "Test log message against resource id.", default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task WriteLogAsync_WithResourceDisplayName_Succeeds()
	{
		// Get the windows device display name from the WindowsDeviceId
		var windowsDevice = await LogicMonitorClient
			.GetAsync<Device>(WindowsDeviceId, default)
			.ConfigureAwait(true);

		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, windowsDevice.DisplayName, "Test log message against resource display name.", default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
	}
}
