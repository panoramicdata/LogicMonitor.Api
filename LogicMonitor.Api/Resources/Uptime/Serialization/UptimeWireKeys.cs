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
	public const string PollingInterval = "uptime.pollingInterval";
	public const string UseDefaultAlertSetting = "uptime.usedefaultalertsetting";
	public const string UseDefaultLocationSetting = "uptime.usedefaultlocationsetting";
	public const string ServiceParameters = "website.private.serviceParameters";

	public const string PingCategory = "pingcheckdevice";
	public const string WebCategory = "webcheckdevice";
}

