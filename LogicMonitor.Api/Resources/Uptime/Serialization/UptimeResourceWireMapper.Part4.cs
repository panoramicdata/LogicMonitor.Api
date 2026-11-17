namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static partial class UptimeResourceWireMapper
{

	private static Level LevelFromWire(string value, Level fallback) => value switch
	{
		"warn" => Level.Warning,
		"error" => Level.Error,
		"critical" => Level.Critical,
		_ => fallback,
	};

	private static string SchemeToWire(UptimeHttpScheme scheme) => scheme == UptimeHttpScheme.Http ? "http" : "https";

	private static UptimeHttpScheme SchemeFromWire(string value, UptimeHttpScheme fallback) => value switch
	{
		"http" => UptimeHttpScheme.Http,
		"https" => UptimeHttpScheme.Https,
		_ => fallback,
	};
}
