namespace LogicMonitor.Api.Converters;

internal class AutomaticUpgradeInfoConverter : JsonCreationConverter<AutomaticUpgradeInfo>
{
	protected override AutomaticUpgradeInfo Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant();
		return type switch
		{
			"automatic upgrade" => new AutomaticUpgradeAutomaticUpgradeInfo(),
			_ => throw new NotSupportedException($"{nameof(AutomaticUpgradeInfoConverter)}.cs needs updating to include include {type}.")
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
#pragma warning disable IDE0022 // Use expression body for methods
		throw new NotSupportedException();
#pragma warning restore IDE0022 // Use expression body for methods
	}
}
