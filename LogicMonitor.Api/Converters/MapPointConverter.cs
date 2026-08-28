namespace LogicMonitor.Api.Converters;

internal class MapPointConverter : JsonCreationConverter<MapPoint>
{
	protected override MapPoint Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant();
		return type switch
		{
			"device" => new ResourceMapPoint(),
			"group" => new ResourceGroupMapPoint(),
			_ => throw new NotSupportedException(
				type is null
					? $"{nameof(MapPointConverter)}: the response contained no 'type' field, so the map point subtype cannot be determined. If you are selecting specific fields, include 'type'."
					: $"{nameof(MapPointConverter)}.cs needs updating to include {type}."),
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotSupportedException();
}
