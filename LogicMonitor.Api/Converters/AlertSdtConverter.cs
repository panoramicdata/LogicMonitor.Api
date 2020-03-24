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
				"resourcesdt" => new DeviceAlertSdt(),
				"devicegroupsdt" => new DeviceGroupAlertSdt(),
				"websitesdt" => new WebsiteAlertSdt(),
				"websitegroupsdt" => new WebsiteGroupAlertSdt(),
				"devicedatasourceinstancesdt" => new DeviceDataSourceInstanceSdt(),
				_ => throw new NotSupportedException($"AlertSdtConverter.cs needs updating to include {type}."),
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();
	}
}
