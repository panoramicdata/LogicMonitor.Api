namespace LogicMonitor.Api.Converters;

/// <summary>
/// Handles JSON properties that can be either a single object or an array.
/// </summary>
internal class SingleOrArrayConverter<T> : JsonConverter<List<T>>
{
	public override List<T>? ReadJson(JsonReader reader, Type objectType, List<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var token = JToken.Load(reader);
		return token.Type switch
		{
			JTokenType.Array => token.ToObject<List<T>>(serializer),
			JTokenType.Object => [token.ToObject<T>(serializer)!],
			JTokenType.Null => [],
			_ => []
		};
	}

	public override void WriteJson(JsonWriter writer, List<T>? value, JsonSerializer serializer)
		=> serializer.Serialize(writer, value);
}
