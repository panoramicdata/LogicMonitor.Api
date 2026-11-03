namespace LogicMonitor.Api.Test.LogicModules;

public class DiagnosticRemediationTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private async Task<int> GetAnyResourceIdAsync()
	{
		var resources = await LogicMonitorClient.GetPageAsync(new Filter<Resource> { Take = 1 }, CancellationToken);
		resources.Items.Should().NotBeEmpty();
		return resources.Items[0].Id;
	}

	[Fact]
	public async Task GetAllRemediationSources_Succeeds()
	{
		var remediationSources = await LogicMonitorClient.GetAllAsync<RemediationSource>(CancellationToken);

		remediationSources.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAssignedSources_ForResource_Succeeds()
	{
		var page = await LogicMonitorClient.GetDiagnosticRemediationAssignedSourcesAsync(await GetAnyResourceIdAsync(), null, CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetExecutionResults_ForResource_Succeeds()
	{
		var page = await LogicMonitorClient.GetDiagnosticRemediationExecutionResultsAsync(null, await GetAnyResourceIdAsync(), 0, 10, CancellationToken);

		page.Should().NotBeNull();
		page.Items.Should().NotBeNull();
	}

	[Fact]
	public Task GetExecutionResults_WithBothTargets_Throws()
	{
		var act = () => LogicMonitorClient.GetDiagnosticRemediationExecutionResultsAsync("DS1", WindowsDeviceId, 0, 10, CancellationToken);

		return act.Should().ThrowAsync<ArgumentException>();
	}
}
