namespace LogicMonitor.Api.Converters;

/// <summary>
/// Deserialize the double/string combination that comes from LogicMonitor
/// It's a double unless a data point is missing, in which case it is the string "N/A"
/// </summary>
internal class DoubleOrNAConverter : JsonConverter<double>
{
	/// <summary>
	/// Read a double from a JsonReader, handling non-double values returned by LogicMonitor
	/// </summary>
	/// <param name="reader">The reader from which to extract the value</param>
	/// <param name="objectType">The type of object being deserialized</param>
	/// <param name="existingValue">The existing value for this item</param>
	/// <param name="hasExistingValue">Whether the item has an existing value</param>
	/// <param name="serializer">A serializer for use in the conversion, if required</param>
	/// <returns>A double, or double.MinValue where the data cannot be deserialized</returns>
	public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.Value == null)
		{
			return double.MinValue;
		}

		var valueString = reader.Value.ToString();
		if (double.TryParse(valueString, out var result))
		{
			return result;
		}

		return double.MinValue;
	}

	/// <summary>
	/// Write a value into a JsonWriter
	/// </summary>
	/// <param name="writer">The writer to be written to the writer</param>
	/// <param name="value">The value to be serialized</param>
	/// <param name="serializer">A serializer for use in the conversion, if required</param>
	public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
		=> writer.WriteValue(value);
}
