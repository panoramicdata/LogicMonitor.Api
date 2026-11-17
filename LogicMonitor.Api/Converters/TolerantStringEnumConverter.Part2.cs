namespace LogicMonitor.Api.Converters;

/// <summary>
/// Tolerant enum converter that handles unmatched values by falling back to a default.
/// Falls back to "Unknown" if present, then "All", then the first enum value (0).
/// </summary>
internal partial class TolerantStringEnumConverter
{

	/// <summary>
	/// Resolves a JSON integer to an enum member by its underlying value.
	/// </summary>
	private static object? ReadIntegerToken(object? readerValue, Type enumType, bool isNullable, string? defaultName)
	{
		var enumVal = Convert.ToInt32(readerValue, CultureInfo.InvariantCulture);
		var values = (int[])Enum.GetValues(enumType);
		if (values.Contains(enumVal))
		{
			return Enum.Parse(enumType, enumVal.ToString(CultureInfo.InvariantCulture));
		}

		if (isNullable)
		{
			return null;
		}

		if (defaultName is not null)
		{
			return Enum.Parse(enumType, defaultName);
		}

		throw new FormatException($"Unsupported integer for {enumType.Name}: {enumVal} and {enumType} has no 'Unknown' member.");
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => writer.WriteValue(value?.ToString());

	private static bool IsNullableType(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
}
