using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26936 Pins the class each report type converts to, so modelling a new type cannot move an existing one.
/// </summary>
public class ReportConverterTests
{
	public static TheoryData<string, Type> ModelledTypes() => new()
	{
		{ "Advanced Metrics", typeof(AdvancedMetricsReport) },
		{ "Alert", typeof(AlertsReport) },
		{ "Alert Forecasting", typeof(AlertForecastReport) },
		{ "Alert SLA", typeof(AlertSlaReport) },
		{ "Alert threshold", typeof(AlertsThresholdsReport) },
		{ "Alert trends", typeof(AlertTrendsReport) },
		{ "AlertsHealthCheck", typeof(AlertsHealthCheckReport) },
		{ "Audit Log", typeof(AuditLogReport) },
		{ "Dashboard", typeof(DashboardReport) },
		{ "Host CPU", typeof(ServerCpuReport) },
		{ "Host group inventory", typeof(ResourceGroupInventoryReport) },
		{ "Host inventory", typeof(ResourceInventoryReport) },
		{ "Host metric trends", typeof(ResourceMetricTrendsReport) },
		{ "Interfaces Bandwidth", typeof(InterfaceBandwidthReport) },
		{ "Netflow device metric", typeof(NetflowResourceMetricReport) },
		{ "Role", typeof(RoleReport) },
		{ "Service Level Agreement", typeof(SlaReport) },
		{ "Uptime Resource Overview", typeof(UptimeResourceOverviewReport) },
		{ "User", typeof(UserReport) },
		{ "Website Service Overview", typeof(WebsiteOverviewReport) },
		{ "Website SLA", typeof(WebsiteSlaReport) },
		{ "Word template", typeof(WordTemplateReport) },
	};

	[Theory]
	[MemberData(nameof(ModelledTypes))]
	public void EachModelledType_ConvertsToItsOwnClass(string type, Type expectedClass)
	{
		var report = JsonConvert.DeserializeObject<ReportBase>($$"""{"id": 1, "name": "Report", "type": "{{type}}"}""");

		report.Should().BeOfType(expectedClass);
	}
}
