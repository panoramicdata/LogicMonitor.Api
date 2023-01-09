namespace LogicMonitor.Api.Test.Statistics;

public class StatisticsTests : TestWithOutput
{
	public StatisticsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task Statistics_GetVersion_Succeeds()
	{
		var _ = await LogicMonitorClient
			.GetVersionAsync(CancellationToken.None)
			.ConfigureAwait(false);
		Api.Statistics statistics = LogicMonitorClient.Statistics;
		statistics.ApiCallSuccessCount.Should().Be(1);
		statistics.ApiCallFailureCount.Should().Be(0);
		statistics.ApiCallPostCount.Should().Be(0);
		statistics.ApiCallGetCount.Should().Be(1);
		statistics.ApiCallDeleteCount.Should().Be(0);
		statistics.ApiCallTraceCount.Should().Be(0);
		statistics.ApiCallHeadCount.Should().Be(0);
		statistics.ApiCallPutCount.Should().Be(0);
		statistics.ApiCallOptionsCount.Should().Be(0);
		statistics.ApiCallPatchCount.Should().Be(0);
		statistics.ApiCallOtherCount.Should().Be(0);
		statistics.DataTransferUplinkBytes.Should().Be(0); // Simple get with no content
		statistics.DataTransferDownlinkBytes.Should().BePositive();
	}

	[Fact]
	public async Task Statistics_GetCollectors_Succeeds()
	{
		var _ = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken.None)
			.ConfigureAwait(false);
		Api.Statistics statistics = LogicMonitorClient.Statistics;
		statistics.ApiCallSuccessCount.Should().Be(1);
		statistics.ApiCallFailureCount.Should().Be(0);
		statistics.ApiCallPostCount.Should().Be(0);
		statistics.ApiCallGetCount.Should().Be(1);
		statistics.ApiCallDeleteCount.Should().Be(0);
		statistics.ApiCallTraceCount.Should().Be(0);
		statistics.ApiCallHeadCount.Should().Be(0);
		statistics.ApiCallPutCount.Should().Be(0);
		statistics.ApiCallOptionsCount.Should().Be(0);
		statistics.ApiCallPatchCount.Should().Be(0);
		statistics.ApiCallOtherCount.Should().Be(0);
		statistics.DataTransferUplinkBytes.Should().Be(0); // Simple get with no content
		statistics.DataTransferDownlinkBytes.Should().BePositive();
	}
}