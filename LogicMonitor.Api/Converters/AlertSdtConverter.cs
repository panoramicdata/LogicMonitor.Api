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
			"DEVICEBATCHJOBSDT" => new ResourceBatchJobAlertSdt(),
			"DEVICECLUSTERALERTDEFSDT" => new ResourceClusterAlertDefSdt(),
			"DEVICEDATASOURCEINSTANCESDT" => new ResourceDataSourceInstanceAlertSdt(),
			"DEVICEDATASOURCEINSTANCEGROUPSDT" => new ResourceDataSourceInstanceGroupAlertSdt(),
			"DEVICEDATASOURCESDT" => new ResourceDataSourceAlertSdt(),
			"DEVICEEVENTSOURCESDT" => new ResourceEventSourceAlertSdt(),
			"DEVICEGROUPSDT" or "RESOURCEGROUPSDT" => new ResourceGroupAlertSdt(),
			"RESOURCESDT" => new ResourceAlertSdt(),
			"SERVICESDT" => new ServiceAlertSdt(),
			"WEBSITESDT" => new WebsiteAlertSdt(),
			"WEBSITECHECKPOINTSDT" => new WebsiteCheckpointAlertSdt(),
			"WEBSITEGROUPSDT" => new WebsiteGroupAlertSdt(),
			_ => throw new NotSupportedException($"AlertSdtConverter.cs needs updating to include {type}."),
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		=> throw new NotSupportedException();
}
