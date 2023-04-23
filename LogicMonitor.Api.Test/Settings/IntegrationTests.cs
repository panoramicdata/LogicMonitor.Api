namespace LogicMonitor.Api.Test.Settings;

public class IntegrationTests : TestWithOutput
{
	public IntegrationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetIntegrations()
	{
		var integrations = await LogicMonitorClient.GetAllAsync<Integration>(default).ConfigureAwait(false);

		// Text should be set
		integrations.Should().AllSatisfy(on => string.IsNullOrWhiteSpace(on.Name).Should().BeFalse());
	}

	[Fact]
	public async Task GetIntegrationAuditLogs()
	{
		var auditLogs = await LogicMonitorClient
			.GetIntegrationAuditLogsAsync()
			.ConfigureAwait(false);

		auditLogs.Items.Should().NotBeEmpty();
	}
}
