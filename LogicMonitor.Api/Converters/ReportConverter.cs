using LogicMonitor.Api.Reports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters
{
	/// <summary>
	/// A report converter
	/// </summary>
	internal class ReportConverter : JsonCreationConverter<Report>
	{
		protected override Report Create(Type objectType, JObject jObject)
		{
			var type = jObject["type"].Value<string>().ToLowerInvariant();

			switch (type)
			{
				case "dashboard":
					return new DashboardReport();
				case "alert":
					return new AlertsReport();
				case "alert forecasting":
					return new AlertForecastReport();
				case "alert sla":
					return new AlertSlaReport();
				case "alert threshold":
					return new AlertsThresholdsReport();
				case "alert trends":
					return new AlertTrendsReport();
				case "audit log":
					return new AuditLogReport();
				case "host cpu":
					return new ServerCpuReport();
				case "host group inventory":
					return new DeviceGroupInventoryReport();
				case "host inventory":
					return new DeviceInventoryReport();
				case "host metric trends":
					return new DeviceMetricTrendsReport();
				case "interfaces bandwidth":
					return new InterfacesBandwidthReport();
				case "netflow device metric":
					return new NetflowDeviceMetricReport();
				case "role":
					return new RoleReport();
				case "service level agreement":
					return new SlaReport();
				case "user":
					return new UserReport();
				case "website service overview":
					return new WebsiteOverviewReport();
				case "website sla":
					return new WebsiteSlaReport();
				case "word template":
					return new WordTemplateReport();
				default:
					throw new NotSupportedException($"ReportConverter.cs needs updating to include {type} reports.");
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			=> throw new NotSupportedException();
	}
}