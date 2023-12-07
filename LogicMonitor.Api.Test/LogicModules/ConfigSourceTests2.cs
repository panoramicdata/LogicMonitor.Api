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
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		configSources.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		configSources.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetConfigSourceById()
	{
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(default)
			.ConfigureAwait(true);
		configSources.Should().NotBeNullOrEmpty();
		var configSource = await LogicMonitorClient
			.GetAsync<ConfigSource>(configSources[0].Id, default)
			.ConfigureAwait(true);
		configSource.Should().NotBeNull();
	}

	[Fact]
	public async Task UpdateAuditVersion()
	{
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(default)
			.ConfigureAwait(true);
		configSources.Should().NotBeNullOrEmpty();

		var original = configSources[0];
		var defaultVersion = original.AuditVersion;

		await LogicMonitorClient
			.AddConfigSourceAuditVersionAsync(original.Id, new Audit() { Version = 1 }, default)
			.ConfigureAwait(true);

		var configSource = await LogicMonitorClient
			.GetAsync<ConfigSource>(original.Id, default)
			.ConfigureAwait(true);

		configSource.AuditVersion.Should().Be(1);

		await LogicMonitorClient
			.AddConfigSourceAuditVersionAsync(original.Id, new Audit() { Version = defaultVersion }, default)
			.ConfigureAwait(true);
	}
}
