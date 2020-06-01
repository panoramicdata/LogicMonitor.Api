using LogicMonitor.Api.Alerts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters
{
	internal class AlertSdtConverter : JsonCreationConverter<AlertSdt>
	{
		protected override AlertSdt Create(Type objectType, JObject jObject)
		{
			var type = jObject["type"].Value<string>().ToLowerInvariant();
			return type switch
			{
				"collectorsdt" => new CollectorAlertSdt(),
				"devicebatchjobsdt" => new DeviceBatchJobAlertSdt(),
				"deviceclusteralertdefsdt" => new DeviceClusterAlertDefSdt(),
				"devicedatasourceinstancesdt" => new DeviceDataSourceInstanceAlertSdt(),
				"devicedatasourceinstancegroupsdt" => new DeviceDataSourceInstanceGroupAlertSdt(),
				"devicedatasourcesdt" => new DeviceDataSourceAlertSdt(),
				"deviceeventsourcesdt" => new DeviceEventSourceAlertSdt(),
				"devicegroupsdt" => new DeviceGroupAlertSdt(),
				"resourcesdt" => new DeviceAlertSdt(),
				"servicesdt" => new ServiceAlertSdt(),
				"websitesdt" => new WebsiteAlertSdt(),
				"websitecheckpointsdt" => new WebsiteCheckpointAlertSdt(),
				"websitegroupsdt" => new WebsiteGroupAlertSdt(),
				_ => throw new NotSupportedException($"AlertSdtConverter.cs needs updating to include {type}."),
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();
	}
}
