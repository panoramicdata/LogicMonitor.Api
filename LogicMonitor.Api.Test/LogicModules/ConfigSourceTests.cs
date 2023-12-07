namespace LogicMonitor.Api.Test.LogicModules;

public class ConfigSourceTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Obsolete("Tests obsolete items")]
	[Fact]
	public async Task GetXml()
	{
		var eventSource = await LogicMonitorClient
			.GetConfigSourceByNameAsync("Test ConfigSource", default)
			.ConfigureAwait(true);
		eventSource ??= new();
		var xml = await LogicMonitorClient
			.GetConfigSourceXmlAsync(eventSource.Id, default)
			.ConfigureAwait(true);

		xml.Should().NotBeNull();
	}
}