using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26846 Host metric trends and Interfaces Bandwidth reports read topN, which live portals send as "all".
/// </summary>
public class ReportTopNTests
{
	private static T Read<T>(string type) where T : ReportBase
		=> JsonConvert.DeserializeObject<ReportBase>(
			$$"""{"id": 1, "name": "Thing", "type": "{{type}}", "topN": "all"}""",
			new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error })
			.Should().BeOfType<T>().Subject;

	[Fact]
	public void HostMetricTrends_ReadsTopN()
		=> Read<ResourceMetricTrendsReport>("Host metric trends").TopN.Should().Be("all");

	[Fact]
	public void InterfacesBandwidth_ReadsTopN()
		=> Read<InterfaceBandwidthReport>("Interfaces Bandwidth").TopN.Should().Be("all");
}
