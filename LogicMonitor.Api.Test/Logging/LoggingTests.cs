using LogicMonitor.Api.Logging;
using System.Globalization;

namespace LogicMonitor.Api.Test.Logging;

public class LoggingTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task WriteLogAsync_WithResourceId_Succeeds()
	{
		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, WindowsDeviceId, "Test log message against resource id.", CancellationToken);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task WriteLogAsync_WithResourceDisplayName_Succeeds()
	{
		// Get the windows device display name from the WindowsDeviceId
		var windowsDevice = await LogicMonitorClient
			.GetAsync<Resource>(WindowsDeviceId, CancellationToken);

		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, windowsDevice.DisplayName, "Test log message against resource display name.", CancellationToken);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task WriteLogAsync_WithCustomProperties_Succeeds()
	{
		// Get the device custom properties
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(WindowsDeviceId, CancellationToken);

		// Get the cmdb.id
		var cmdbId = deviceProperties
			.FirstOrDefault(dp => dp.Name == "cmdb.id")?.Value
			?? throw new FormatException("For this unit test to work, a unique cmdb.id custom property should be configured");

		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, "cmdb.id", cmdbId, "Test log message against resource cmdb.id custom property.", CancellationToken);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task WriteLogAsync_WithDictionary_Succeeds()
	{
		// Get the windows device display name from the WindowsDeviceId
		var windowsDevice = await LogicMonitorClient
			.GetAsync<Resource>(WindowsDeviceId, CancellationToken);

		// Get the device custom properties
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(WindowsDeviceId, CancellationToken);

		// Get the cmdb.id
		var cmdbId = deviceProperties
			.FirstOrDefault(dp => dp.Name == "cmdb.id")?.Value
			?? throw new FormatException("For this unit test to work, a unique cmdb.id custom property should be configured");

		var response = await LogicMonitorClient
			.WriteLogAsync(WriteLogLevel.Info, new Dictionary<string, string>
			{
				{ "system.deviceId", WindowsDeviceId.ToString(CultureInfo.InvariantCulture) },
				{ "system.displayname", windowsDevice.DisplayName },
				{ "cmdb.id", cmdbId },
			},
			"Test log message against a dictionary of resource id, displayname and cmdb.id custom property.", CancellationToken);
		response.Should().NotBeNull();
	}
}
