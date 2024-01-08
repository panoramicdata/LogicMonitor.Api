namespace LogicMonitor.Api.Test.Settings;

public class IntegrationTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetIntegrations()
	{
		var integrations = await LogicMonitorClient
			.GetAllAsync<Integration>(default)
			.ConfigureAwait(true);

		// Text should be set
		integrations.Should().AllSatisfy(on => on.Name.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task GetIntegrationAuditLogs()
	{
		var auditLogs = await LogicMonitorClient
			.GetIntegrationAuditLogsAsync()
			.ConfigureAwait(true);

		auditLogs.Items.Should().NotBeEmpty();
	}
}
