namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The HTTP scheme used by an Uptime web check.
/// Uptime-owned so that the new surface stays decoupled from the legacy <c>Websites</c> namespace.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum UptimeHttpScheme
{
	/// <summary>
	/// HTTP.
	/// </summary>
	[EnumMember(Value = "http")]
	Http = 0,

	/// <summary>
	/// HTTPS.
	/// </summary>
	[EnumMember(Value = "https")]
	Https = 1,
}

