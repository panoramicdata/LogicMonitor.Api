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
		Assert.NotNull((object)portalVersion.Version);
		Assert.NotNull((object)portalVersion.Version.Module);
		Assert.NotNull(portalVersion.Extra);
		Assert.NotNull(portalVersion.Hash);
		Assert.NotNull(portalVersion.BuildAt);
		Assert.NotNull(portalVersion.Branch);
		Assert.NotNull(portalVersion.ResultKey);
	}

	[Fact]
	public async void GetPortalVersionStatic()
	{
		var portalVersion = await Api.LogicMonitorClient.GetVersionAsync("altius").ConfigureAwait(false);
		Assert.NotNull((object)portalVersion.Version);
		Assert.NotNull((object)portalVersion.Version.Module);
		Assert.NotNull(portalVersion.Extra);
		Assert.NotNull(portalVersion.Hash);
		Assert.NotNull(portalVersion.BuildAt);
		Assert.NotNull(portalVersion.Branch);
		Assert.NotNull(portalVersion.ResultKey);
	}
}
