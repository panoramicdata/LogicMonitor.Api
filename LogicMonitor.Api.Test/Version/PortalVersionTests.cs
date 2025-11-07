namespace LogicMonitor.Api.Test.Version;

public class PortalVersionTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetPortalVersion()
	{
		var portalVersion = await LogicMonitorClient.GetVersionAsync(CancellationToken);
		portalVersion.Version.Should().NotBeNull();
		portalVersion.Version.Module.Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}

	[Fact]
	public async Task GetPortalVersionStatic()
	{
		var portalVersion = await LogicMonitorClient
			.GetVersionAsync("panoramicdata", CancellationToken);
		portalVersion.Version.Should().NotBeNull();
		((object)portalVersion.Version.Module).Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}
}
