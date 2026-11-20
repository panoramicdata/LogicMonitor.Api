namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static partial class UptimeResourceWireMapper
{

	private static UptimeTestLocation ReadTestLocation(JToken? token)
	{
		if (token is not JObject obj)
		{
			return new UptimeTestLocation();
		}

		return new UptimeTestLocation
		{
			All = obj["all"]?.Value<bool>() ?? false,
			CollectorIds = ReadIntList(obj["collectorIds"]),
			SmgIds = ReadIntList(obj["smgIds"]),
		};
	}

	private static List<int> ReadIntList(JToken? token)
		=> token is JArray array ? array.Select(t => t.Value<int>()).ToList() : [];

	private static bool ReadBoolProperty(Dictionary<string, string> properties, string key)
		=> properties.TryGetValue(key, out var value) && ParseBool(value, false);

	private static string GetString(JObject device, string key, string fallback = "")
		=> device[key]?.Type is JTokenType.String or JTokenType.Integer or JTokenType.Float
			? device[key]!.ToString()
			: fallback;

	private static int GetInt(JObject device, string key, int fallback)
		=> device[key] is { } token && int.TryParse(token.ToString(), NumberStyles.Integer, Invariant, out var value)
			? value
			: fallback;

	private static bool GetBool(JObject device, string key, bool fallback)
		=> device[key] is { } token && bool.TryParse(token.ToString(), out var value) ? value : fallback;
}
