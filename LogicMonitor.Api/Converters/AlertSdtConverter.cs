namespace LogicMonitor.Api.Converters;

internal class AlertSdtConverter : JsonCreationConverter<AlertSdt>
{
	protected override AlertSdt Create(Type objectType, JObject jObject)
	{
		var type = (
			(jObject["type"] ?? throw new FormatException("Type is missing"))
				.Value<string>() ?? throw new FormatException("Type should be a string")).ToUpperInvariant();
		return type switch
		{
			"COLLECTORSDT" => new CollectorAlertSdt(),
			"DEVICEBATCHJOBSDT" => new DeviceBatchJobAlertSdt(),
			"DEVICECLUSTERALERTDEFSDT" => new DeviceClusterAlertDefSdt(),
			"DEVICEDATASOURCEINSTANCESDT" => new DeviceDataSourceInstanceAlertSdt(),
			"DEVICEDATASOURCEINSTANCEGROUPSDT" => new DeviceDataSourceInstanceGroupAlertSdt(),
			"DEVICEDATASOURCESDT" => new DeviceDataSourceAlertSdt(),
			"DEVICEEVENTSOURCESDT" => new DeviceEventSourceAlertSdt(),
			"DEVICEGROUPSDT" => new DeviceGroupAlertSdt(),
			"RESOURCESDT" => new DeviceAlertSdt(),
			"SERVICESDT" => new ServiceAlertSdt(),
			"WEBSITESDT" => new WebsiteAlertSdt(),
			"WEBSITECHECKPOINTSDT" => new WebsiteCheckpointAlertSdt(),
			"WEBSITEGROUPSDT" => new WebsiteGroupAlertSdt(),
			_ => throw new NotSupportedException($"AlertSdtConverter.cs needs updating to include {type}."),
		};
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		=> throw new NotSupportedException();
}
