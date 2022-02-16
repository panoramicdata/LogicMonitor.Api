namespace LogicMonitor.Api.Test.LogicModules;

public class ConfigSourceTests : TestWithOutput
{
	public ConfigSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Obsolete("Tests obsolete items")]
	[Fact]
	public async void GetXml()
	{
		var eventSource = await LogicMonitorClient.GetConfigSourceByNameAsync("Test ConfigSource").ConfigureAwait(false);
		var xml = await LogicMonitorClient.GetConfigSourceXmlAsync(eventSource.Id).ConfigureAwait(false);

		xml.Should().NotBeNull();
	}
}