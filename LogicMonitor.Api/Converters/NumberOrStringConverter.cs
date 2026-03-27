namespace LogicMonitor.Api.Converters;

/// <summary>
/// Deserializes a JSON value that may be a number or a string into a C# string.
/// LogicMonitor sometimes returns numeric fields as a JSON number (e.g. 0) and
/// sometimes as a JSON string (e.g. "0.0"), so this converter handles both.
/// </summary>
internal class NumberOrStringConverter : JsonConverter<string>
{
	/// <inheritdoc />
	public override string ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
		=> reader.TokenType switch
		{
			JsonToken.Null => string.Empty,
			JsonToken.Integer or JsonToken.Float => Convert.ToString(reader.Value, CultureInfo.InvariantCulture) ?? string.Empty,
			_ => reader.Value?.ToString() ?? string.Empty,
		};

	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
		=> writer.WriteValue(value);
}
