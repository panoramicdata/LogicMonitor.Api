using System.Globalization;

namespace LogicMonitor.Api.Test.PushMetrics;

public class PushMetricTests : TestWithOutput
{
	public PushMetricTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task PushMetric_Succeeds()
	{
		var response = await LogicMonitorClient
			.PushMetricAsync(new PushMetric
			{
				ResourceIds = new()
				{
					["system.deviceId"] = WindowsDeviceId.ToString(CultureInfo.InvariantCulture)
				},
				DataSourceName = "UnitTest_PushMetric_Succeeds",
				DataSourceDisplayName = "PushMetric_Succeeds",
				DataSourceGroup = "Unit Tests",
				Instances =
				[
					new()
					{
						Name = "Slot1",
						DisplayName = "Slot 1",
						Properties = new()
						{
							["unit_test.slot_id"] = "1"
						},
						DataPoints =
						[
							new()
							{
								Name = "DataPoint1",
								Description = "DataPoint 1",
								//AggregationType = PushMetricAggregationType.Mean,
								DataType = PushMetricDataPointDataType.Counter,
								Values = new Dictionary<DateTimeOffset, int>
								{
									[DateTimeOffset.UtcNow.AddSeconds(-2)] = 10
								}.ToLogicMonitorDictionary()
							}
						]
					}
				]
			}, cancellationToken: default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
	}
}
