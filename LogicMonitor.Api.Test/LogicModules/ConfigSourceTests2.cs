using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test;

public class ConfigSourceTests2 : TestWithOutput
{
	public ConfigSourceTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllConfigSources()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>(default).ConfigureAwait(false);

		// Make sure that some are returned
		configSources.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		configSources.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetConfigSourceById()
	{
		var configSources = await LogicMonitorClient.GetAllAsync<ConfigSource>(default).ConfigureAwait(false);
		configSources.Should().NotBeNullOrEmpty();
		var configSource = await LogicMonitorClient.GetAsync<ConfigSource>(configSources[0].Id, default).ConfigureAwait(false);
		configSource.Should().NotBeNull();
	}

}
