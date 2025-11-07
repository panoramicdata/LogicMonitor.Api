namespace LogicMonitor.Api.Test.Settings;

public class IntegrationTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetIntegrations()
	{
		var integrations = await LogicMonitorClient
			.GetAllAsync<Integration>(CancellationToken);

		// Text should be set
		integrations.Should().AllSatisfy(on => on.Name.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task GetIntegrationAuditLogs()
	{
		var auditLogs = await LogicMonitorClient
			.GetIntegrationAuditLogsAsync(CancellationToken);

		auditLogs.Items.Should().NotBeEmpty();
	}
}
