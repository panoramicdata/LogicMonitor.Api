namespace LogicMonitor.Api.Test.Settings;

public class PortalVersionTests : TestWithOutput
{
	public PortalVersionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetPortalVersion()
	{
		var portalVersion = await LogicMonitorClient.GetVersionAsync().ConfigureAwait(false);
		portalVersion.Version.Should().NotBeNull();
		portalVersion.Version.Module.Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}

	[Fact]
	public async void GetPortalVersionStatic()
	{
		var portalVersion = await Api.LogicMonitorClient.GetVersionAsync("altius").ConfigureAwait(false);
		portalVersion.Version.Should().NotBeNull();
		((object)portalVersion.Version.Module).Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}
}
