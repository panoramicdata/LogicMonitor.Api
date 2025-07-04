namespace LogicMonitor.Api.Converters;

/// <summary>
/// A report converter
/// </summary>
internal class ReportConverter : JsonCreationConverter<ReportBase>
{
	protected override ReportBase Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant();

		return type switch
		{
			"dashboard" => new DashboardReport(),
			"alert" => new AlertsReport(),
			"alert forecasting" => new AlertForecastReport(),
			"alert sla" => new AlertSlaReport(),
			"alert threshold" => new AlertsThresholdsReport(),
			"alert trends" => new AlertTrendsReport(),
			"audit log" => new AuditLogReport(),
			"host cpu" => new ServerCpuReport(),
			"host group inventory" => new ResourceGroupInventoryReport(),
			"host inventory" => new ResourceInventoryReport(),
			"host metric trends" => new ResourceMetricTrendsReport(),
			"interfaces bandwidth" => new InterfaceBandwidthReport(),
			"netflow device metric" => new NetflowResourceMetricReport(),
			"role" => new RoleReport(),
			"service level agreement" => new SlaReport(),
			"user" => new UserReport(),
			"website service overview" => new WebsiteOverviewReport(),
			"website sla" => new WebsiteSlaReport(),
			"word template" => new WordTemplateReport(),
			_ => throw new NotSupportedException($"{nameof(ReportConverter)}.cs needs updating to include {type} reports."),
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		=> throw new NotSupportedException();
}
