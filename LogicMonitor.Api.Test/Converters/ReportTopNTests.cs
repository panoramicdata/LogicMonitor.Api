using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26846, MS-26936, MS-27673 Host metric trends, Interfaces Bandwidth, Alert Forecasting and Host CPU reports read topN, which live portals send as a string such as "all" or "50".
/// </summary>
public class ReportTopNTests
{
	private static T Read<T>(string type, string topN = "all") where T : ReportBase
		=> JsonConvert.DeserializeObject<ReportBase>(
			$$"""{"id": 1, "name": "Thing", "type": "{{type}}", "topN": "{{topN}}"}""",
			new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error })
			.Should().BeOfType<T>().Subject;

	[Fact]
	public void HostMetricTrends_ReadsTopN()
		=> Read<ResourceMetricTrendsReport>("Host metric trends").TopN.Should().Be("all");

	[Fact]
	public void InterfacesBandwidth_ReadsTopN()
		=> Read<InterfaceBandwidthReport>("Interfaces Bandwidth").TopN.Should().Be("all");

	[Fact]
	public void AlertForecasting_ReadsTopN()
		=> Read<AlertForecastReport>("Alert Forecasting", "50").TopN.Should().Be("50");

	[Fact]
	public void HostCpu_ReadsTopN()
		=> Read<ServerCpuReport>("Host CPU", "10").TopN.Should().Be("10");
}
