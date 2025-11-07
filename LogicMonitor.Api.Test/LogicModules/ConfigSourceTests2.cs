using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public class ConfigSourceTests2(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllConfigSources()
	{
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(CancellationToken);

		// Make sure that some are returned
		configSources.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		configSources.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetConfigSourceById()
	{
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(CancellationToken);
		configSources.Should().NotBeNullOrEmpty();
		var configSource = await LogicMonitorClient
			.GetAsync<ConfigSource>(configSources[0].Id, CancellationToken);
		configSource.Should().NotBeNull();
	}

	[Fact]
	public async Task UpdateAuditVersion()
	{
		var configSources = await LogicMonitorClient
			.GetAllAsync<ConfigSource>(CancellationToken);
		configSources.Should().NotBeNullOrEmpty();

		var original = configSources[0];
		var defaultVersion = original.AuditVersion;

		await LogicMonitorClient
			.AddConfigSourceAuditVersionAsync(original.Id, new Audit() { Version = 1 }, CancellationToken);

		var configSource = await LogicMonitorClient
			.GetAsync<ConfigSource>(original.Id, CancellationToken);

		configSource.AuditVersion.Should().Be(1);

		await LogicMonitorClient
			.AddConfigSourceAuditVersionAsync(original.Id, new Audit() { Version = defaultVersion }, CancellationToken);
	}
}
