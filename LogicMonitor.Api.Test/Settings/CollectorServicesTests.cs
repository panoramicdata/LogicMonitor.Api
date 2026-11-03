namespace LogicMonitor.Api.Test.Settings;

public class CollectorServicesTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetCollectorEvents_Succeeds()
	{
		var page = await LogicMonitorClient.GetCollectorEventsAsync(CollectorId, CancellationToken);

		page.Items.Should().NotBeEmpty();
		page.Items.Should().AllSatisfy(e => e.CollectorId.Should().Be(CollectorId));
	}

	[Fact]
	public async Task GetCollectorLogFiles_Succeeds()
	{
		var page = await LogicMonitorClient.GetCollectorLogFilesAsync(CollectorId, CancellationToken);

		page.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCollectorStatusCheck_Succeeds()
	{
		var statusCheck = await LogicMonitorClient.GetCollectorStatusCheckAsync(CollectorId, CancellationToken);

		statusCheck.CheckPoints.Should().NotBeEmpty();
		statusCheck.CheckPoints.Should().AllSatisfy(c => c.Name.Should().NotBeNullOrWhiteSpace());
	}

	[Fact]
	public async Task GetCollectorAgentLogLevels_Succeeds()
	{
		var page = await LogicMonitorClient.GetCollectorAgentLogLevelsAsync(CollectorId, CancellationToken);

		page.Items.Should().NotBeEmpty();

		var first = page.Items[0];
		var single = await LogicMonitorClient.GetCollectorAgentLogLevelAsync(CollectorId, first.Component, CancellationToken);
		single.Component.Should().Be(first.Component);
		single.LogLevel.Should().Be(first.LogLevel);
	}

	[Fact]
	public async Task GetCollectorUpgradeHistory_Succeeds()
	{
		var page = await LogicMonitorClient.GetCollectorUpgradeHistoryAsync(new Filter<CollectorUpgradeHistoryItem> { Take = 5 }, CancellationToken);

		page.Items.Should().NotBeNull();
	}

	// The valid device type keys are undocumented, so this proves the request reaches the sizing validator
	[Fact]
	public async Task CalculateCollectorSizing_UnknownDeviceType_SurfacesPortalValidation()
	{
		var act = () => LogicMonitorClient.CalculateCollectorSizingAsync(
			new CollectorSizingRequest
			{
				Devices = new Dictionary<string, CollectorSizingDeviceCount>
				{
					["not-a-device-type"] = new() { Count = 100, Instances = 10 }
				}
			},
			CancellationToken);

		(await act.Should().ThrowAsync<LogicMonitorApiException>())
			.Which.Message.Should().Contain("Unknown device type: not-a-device-type");
	}
}
