using System.Reflection;

namespace LogicMonitor.Api.Test.Version;

public class PortalVersionTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetPortalVersion()
	{
		var portalVersion = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);
		portalVersion.Version.Should().NotBeNull();
		portalVersion.Version.Module.Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();

		// These should match the current version of this library
		// Get the file version of the LogicMonitor.Api project (not this executing assembly)
		var fileVersion = Assembly.GetAssembly(typeof(LogicMonitorClient))?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

		// Split this into the module and the version
		var split = fileVersion!.Split('.').Select(int.Parse).ToList();

		// The module should be the first part
		portalVersion.Version.Build.Should().Be(split[0]);

		// The version should be the second part
		portalVersion.Version.Major.Should().Be(split[1]);
	}

	[Fact]
	public async Task GetPortalVersionStatic()
	{
		var portalVersion = await LogicMonitorClient
			.GetVersionAsync("panoramicdata", default)
			.ConfigureAwait(true);
		portalVersion.Version.Should().NotBeNull();
		((object)portalVersion.Version.Module).Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}
}
