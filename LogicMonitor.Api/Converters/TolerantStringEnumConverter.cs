namespace LogicMonitor.Api.Converters;

/// <summary>
/// Tolerant enum converter that handles unmatched values by falling back to a default.
/// Falls back to "Unknown" if present, then "All", then the first enum value (0).
/// </summary>
internal class TolerantStringEnumConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		var type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
		return type?.IsEnum ?? false;
	}

	private static string? GetEnumMemberAttrValue(Type type, object enumVal)
	{
		var memberInfos = type.GetMember(enumVal.ToString()!);
		var memberInfo = memberInfos.SingleOrDefault(m => m.Name == enumVal.ToString());
		return memberInfo
			?.GetCustomAttributes(false)
			.OfType<EnumMemberAttribute>()
			.FirstOrDefault()
			?.Value;
	}

	private static string[]? GetEnumMemberAliases(Type type, object enumVal)
	{
		var memberInfos = type.GetMember(enumVal.ToString()!);
		var memberInfo = memberInfos.SingleOrDefault(m => m.Name == enumVal.ToString());
		return memberInfo
			?.GetCustomAttributes(false)
			.OfType<EnumMemberAliasAttribute>()
			.FirstOrDefault()
			?.Aliases;
	}

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		var isNullable = IsNullableType(objectType);
		var enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

		var names = Enum.GetNames(enumType ?? throw new ArgumentException("Null enumType in TolerantStringEnumConverter"));
		var defaultName = Array.Find(names, n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase))
		?? Array.Find(names, n => string.Equals(n, "All", StringComparison.OrdinalIgnoreCase))
		?? Enum.GetName(enumType, 0);

		return reader.TokenType switch
		{
			JsonToken.String => ReadStringToken(reader.Value?.ToString(), enumType, isNullable, defaultName),
			JsonToken.Integer => ReadIntegerToken(reader.Value, enumType, isNullable, defaultName),
			_ => throw new FormatException($"Unsupported tokenType: {reader.TokenType}"),
		};
	}

	/// <summary>
	/// Resolves a JSON string to an enum member, by EnumMember value first and then by alias.
	/// </summary>
	private static object? ReadStringToken(string? enumText, Type enumType, bool isNullable, string? defaultName)
	{
		var matched = MatchByEnumMemberOrAlias(enumText, enumType);
		if (matched is not null)
		{
			return matched;
		}

		if (isNullable)
		{
			return null;
		}

		if (defaultName is not null)
		{
#if DEBUG
			throw new NotImplementedException($"{enumType} missing an enum member for {enumText}");
#else
			return Enum.Parse(enumType, defaultName);
#endif
		}

		throw new FormatException($"Unsupported string for {enumType.Name}: {enumText}");
	}

	private static object? MatchByEnumMemberOrAlias(string? enumText, Type enumType)
	{
		if (string.IsNullOrEmpty(enumText))
		{
			return null;
		}

		// Get the right member using the EnumMember attribute
		var match = Array.Find(Enum
			.GetNames(enumType), name => GetEnumMemberAttrValue(enumType, Enum.Parse(enumType, name)) == enumText);

		if (match is not null)
		{
			return Enum.Parse(enumType, match);
		}

		// Check for aliases if primary EnumMember value didn't match
		var aliasMatch = Array.Find(Enum
			.GetNames(enumType), name =>
			{
				var aliases = GetEnumMemberAliases(enumType, Enum.Parse(enumType, name));
				return aliases?.Contains(enumText) == true;
			});

		return aliasMatch is null ? null : Enum.Parse(enumType, aliasMatch);
	}

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
