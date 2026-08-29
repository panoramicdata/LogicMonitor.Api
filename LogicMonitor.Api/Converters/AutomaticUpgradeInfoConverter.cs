namespace LogicMonitor.Api.Converters;

internal class AutomaticUpgradeInfoConverter : JsonCreationConverter<AutomaticUpgradeInfo>
{
	protected override AutomaticUpgradeInfo Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant();
		return type == "automatic upgrade"
			? new AutomaticUpgradeAutomaticUpgradeInfo()
			: throw new NotSupportedException(
				type is null
					? $"{nameof(AutomaticUpgradeInfoConverter)}: the response contained no 'type' field, so the automatic upgrade info subtype cannot be determined. If you are selecting specific fields, include 'type'."
					: $"{nameof(AutomaticUpgradeInfoConverter)}.cs needs updating to include {type}.");
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
#pragma warning disable IDE0022 // Use expression body for methods
		throw new NotSupportedException();
#pragma warning restore IDE0022 // Use expression body for methods
	}
}
