namespace LogicMonitor.Api.Converters;

/// <summary>
/// Converter for WidgetData, used to handle custom widget data types
/// </summary>
internal class WidgetDataConverter : JsonCreationConverter<WidgetData>
{
	protected override WidgetData Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant()
			?? throw new ArgumentNullException("type", "Type not available for discrimination!");
		return type switch
		{
			"bignumber" => new WidgetData(),
			"cgraph" => new CustomGraphWidgetData(),
			"graph" => new CustomGraphWidgetData(),
			"noc" => new NOCWidgetData(),
			"dynamictable" => new WidgetData(),
			_ => new WidgetData(),
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		=> throw new NotSupportedException();
}
