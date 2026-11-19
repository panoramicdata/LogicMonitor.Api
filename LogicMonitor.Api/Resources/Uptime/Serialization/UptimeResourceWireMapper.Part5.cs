namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static partial class UptimeResourceWireMapper
{

	private static int ParseInt(string value, int fallback)
		=> int.TryParse(value, NumberStyles.Integer, Invariant, out var result) ? result : fallback;

	private static bool ParseBool(string value, bool fallback)
		=> bool.TryParse(value, out var result) ? result : fallback;

	private static bool TryParse<T>(string json, out T? value)
	{
		try
		{
			value = JsonConvert.DeserializeObject<T>(json);
			return value is not null;
		}
		catch (JsonException)
		{
			value = default;
			return false;
		}
	}

	private static string BoolToWire(bool value) => value ? "true" : "false";

	private static string LevelToWire(Level level) => level switch
	{
		Level.Warning => "warn",
		Level.Error => "error",
		Level.Critical => "critical",
		_ => "warn",
	};
}
