namespace LogicMonitor.Api.Test.LogicModules;

public class ConfigSourceTests : TestWithOutput
{
	public ConfigSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

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