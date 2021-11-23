using Xunit;
using Xunit.Abstractions;

// Older, now deprecated methods are still tested here
#pragma warning disable 618

namespace LogicMonitor.Api.Test.LogicModules;

public class ConfigSourceTests : TestWithOutput
{
	public ConfigSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetXml()
	{
		var eventSource = await LogicMonitorClient.GetConfigSourceByNameAsync("Test ConfigSource").ConfigureAwait(false);
		var xml = await LogicMonitorClient.GetConfigSourceXmlAsync(eventSource.Id).ConfigureAwait(false);

		Assert.NotNull(xml);
	}
}

#pragma warning restore 618