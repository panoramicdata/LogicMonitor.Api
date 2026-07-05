namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// The well-known custom-property keys and category values used to encode an Uptime check on the
/// <c>device/devices</c> endpoint. Centralised here so the create and round-trip paths agree, and so the
/// wire format can be corrected in one place if the portal expects something different.
/// </summary>
internal static class UptimeWireKeys
{
	public const string SystemCategories = "system.categories";
	public const string Hostname = "uptime.hostname";
	public const string Url = "uptime.url";
	public const string PollingInterval = "uptime.pollingInterval";
	public const string UseDefaultAlertSetting = "uptime.usedefaultalertsetting";
	public const string UseDefaultLocationSetting = "uptime.usedefaultlocationsetting";
	public const string ServiceParameters = "website.private.serviceParameters";

	/// <summary>
	/// The property that lists the internal Collectors an internal Uptime check runs from. Its presence is
	/// what causes LogicMonitor to set the read-only <c>system.uptime.type = internal</c> system property and
	/// render the internal-collector checkpoints in the portal. Must be set <em>after</em> creation (it needs
	/// the created device id and the referenced Collectors' metadata) — the creation POST body does not persist it.
	/// Format: <c>[{"&lt;collectorId&gt;": ["&lt;description&gt;", "&lt;hostname&gt;", &lt;groupId&gt;, "&lt;groupName&gt;", "alive"], ...}]</c>
	/// </summary>
	public const string Checkpoints = "website.private.checkpoints";

	public const string PingCategory = "pingcheckdevice";
	public const string WebCategory = "webcheckdevice";
}

