using LogicMonitor.Api.Dashboards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters
{
	internal class WidgetConverter : JsonCreationConverter<Widget>
	{
		protected override Widget Create(Type objectType, JObject jObject)
		{
			var type = jObject["type"].Value<string>().ToLowerInvariant();
			switch (type)
			{
				case "alert":
					return new AlertWidget();

				case "bignumber":
					return new BigNumberWidget();

				case "cgraph":
					return new CustomGraphWidget();

				case "devicenoc":
					return new DeviceNocWidget();

				case "devicesla":
					return new DeviceSlaWidget();

				case "flash":
					return new FlashWidget();

				case "gauge":
					return new GaugeWidget();

				case "gmap":
					return new GoogleMapWidget();

				case "html":
					return new HtmlWidget();

				case "batchjob":
					return new JobMonitorWidget();

				case "netflow":
					return new NetflowWidget();

				case "ngraph":
					return new NGraphWidget();

				case "noc":
					return new NocWidget();

				case "ograph":
					return new OverviewGraphWidget();

				case "piechart":
					return new PieChartWidget();

				case "websiteindividualstatus":
					return new WebsiteIndividualStatusWidget();

				case "swebsitenoc":
					return new WebsiteNocWidget();

				case "websitesla":
					return new WebsiteSlaWidget();

				case "websiteoverview":
					return new WebsiteOverviewWidget();

				case "websiteoverallstatus":
					return new WebsiteOverallStatusWidget();

				case "sgraph":
					return new WebsiteGraphWidget();

				case "table":
					return new TableWidget();

				case "text":
					return new TextWidget();

				case "netflowgraph":
					return new NetflowGraphWidget();

				case "groupnetflow":
					return new GroupNetflowWidget();

				case "dynamictable":
					return new DynamicTableWidget();

				default:
					throw new NotSupportedException($"WidgetConverter.cs needs updating to include {type} widgets.");
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			=> throw new NotSupportedException();
	}
}