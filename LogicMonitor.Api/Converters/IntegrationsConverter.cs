namespace LogicMonitor.Api.Converters;

internal class IntegrationsConverter : JsonCreationConverter<Integration>
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();

	protected override Integration Create(Type objectType, JObject jObject)
	{
		var integration = jObject["type"].Value<string>().ToLowerInvariant();
		return integration switch
		{
			"slack" => new SlackIntegration(),
			"http" => new HttpIntegration(),
			"servicenow" => new ServiceNowIntegration(),
			"email" => new EmailIntegration(),
			"autotask" => new AutoTaskIntegration(),
			"pagerduty" => new PagerDutyIntegration(),
			_ => throw new NotSupportedException($"{integration} deserialization not supported.  IntegrationsConverter.cs needs updating."),
		};
	}
}
