namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static partial class UptimeResourceWireMapper
{

	/// <summary>
	/// Builds a strongly-typed resource from a device JSON response.
	/// </summary>
	public static UptimeResource Read(JObject device, Type objectType)
	{
		var resource = CreateInstance(device, objectType);
		PopulateCommon(resource, device);

		if (resource is PingCheckResource ping)
		{
			PopulatePing(ping, device);
		}
		else if (resource is WebCheckResource web)
		{
			PopulateWeb(web, device);
		}

		return resource;
	}

	private static UptimeResource CreateInstance(JObject device, Type objectType)
	{
		if (objectType == typeof(PingCheckResource))
		{
			return new PingCheckResource();
		}

		if (objectType == typeof(WebCheckResource))
		{
			return new WebCheckResource();
		}

		// Abstract / base requested - discriminate on deviceType.
		var deviceType = GetInt(device, "deviceType", 0);
		return deviceType switch
		{
			(int)ResourceType.Ping => new PingCheckResource(),
			(int)ResourceType.Web => new WebCheckResource(),
			_ => throw new NotSupportedException($"Unable to map device of type {deviceType} to an {nameof(UptimeResource)}."),
		};
	}

	private static void PopulateCommon(UptimeResource resource, JObject device)
	{
		var customProperties = ReadCustomProperties(device);

		resource.Id = GetInt(device, "id", 0);
		resource.Name = GetString(device, "name");
		resource.DisplayName = GetString(device, "displayName");
		resource.Description = GetString(device, "description");
		resource.ResourceGroupIds = GetString(device, "hostGroupIds");
		resource.PreferredCollectorId = GetInt(device, "preferredCollectorId", 0);
		resource.DisableAlerting = GetBool(device, "disableAlerting", false);
		resource.IsInternal = GetBool(device, "isInternal", true);
		resource.SyntheticsCollectorIds = ReadIntList(device["syntheticsCollectorIds"]);
		resource.TestLocation = ReadTestLocation(device["testLocation"]);

		// Host name / polling interval can appear either as a custom property or a top-level field.
		resource.HostName = customProperties.TryGetValue(UptimeWireKeys.Hostname, out var host) && !string.IsNullOrEmpty(host)
			? host
			: GetString(device, "host", GetString(device, "domain"));

		resource.PollingIntervalMinutes = customProperties.TryGetValue(UptimeWireKeys.PollingInterval, out var pollingInterval) && int.TryParse(pollingInterval, out var pollingIntervalValue)
			? pollingIntervalValue
			: GetInt(device, "pollingInterval", resource.PollingIntervalMinutes);

		resource.UseDefaultAlertSetting = ReadBoolProperty(customProperties, UptimeWireKeys.UseDefaultAlertSetting);
		resource.UseDefaultLocationSetting = ReadBoolProperty(customProperties, UptimeWireKeys.UseDefaultLocationSetting);

		resource.Alerting = new UptimeAlertSettings
		{
			OverallAlertLevel = LevelFromWire(GetString(device, "overallAlertLevel"), Level.Warning),
			IndividualAlertLevel = LevelFromWire(GetString(device, "individualAlertLevel"), Level.Warning),
			IndividualCheckpointAlertsEnabled = GetBool(device, "individualSmAlertEnable", false),
			FailedCheckCountBeforeAlerting = GetInt(device, "transition", 1),
			AlertCondition = (SiteMonitorAlertCondition)GetInt(device, "globalSmAlertCond", 0),
		};
	}

	private static void PopulatePing(PingCheckResource ping, JObject device)
	{
		// Prefer the nested serviceParameters blob (the create wire shape); fall back to top-level fields.
		var customProperties = ReadCustomProperties(device);
		if (customProperties.TryGetValue(UptimeWireKeys.ServiceParameters, out var serviceParametersJson)
			&& !string.IsNullOrWhiteSpace(serviceParametersJson)
			&& TryParse<UptimePingServiceParameters>(serviceParametersJson, out var serviceParameters))
		{
			ping.PacketCount = ParseInt(serviceParameters!.Count, ping.PacketCount);
			ping.TimeoutMs = ParseInt(serviceParameters.TimeoutInMSPktsNotReceive, ping.TimeoutMs);
			ping.PercentPacketsNotReceivedInTime = ParseInt(serviceParameters.PercentPktsNotReceiveInTime, ping.PercentPacketsNotReceivedInTime);

			ping.Alerting.OverallAlertLevel = LevelFromWire(serviceParameters.OverallAlertLevel, ping.Alerting.OverallAlertLevel);
			ping.Alerting.IndividualAlertLevel = LevelFromWire(serviceParameters.IndividualAlertLevel, ping.Alerting.IndividualAlertLevel);
			ping.Alerting.IndividualCheckpointAlertsEnabled = ParseBool(serviceParameters.IndividualSmAlertEnable, ping.Alerting.IndividualCheckpointAlertsEnabled);
			ping.Alerting.FailedCheckCountBeforeAlerting = ParseInt(serviceParameters.Transition, ping.Alerting.FailedCheckCountBeforeAlerting);
			ping.Alerting.AlertCondition = (SiteMonitorAlertCondition)ParseInt(serviceParameters.GlobalSmAlertCond, (int)ping.Alerting.AlertCondition);

			if (!string.IsNullOrEmpty(serviceParameters.Dns))
			{
				ping.HostName = serviceParameters.Dns;
			}

			return;
		}

		ping.PacketCount = GetInt(device, "count", ping.PacketCount);
		ping.TimeoutMs = GetInt(device, "timeoutInMSPktsNotReceive", ping.TimeoutMs);
		ping.PercentPacketsNotReceivedInTime = GetInt(device, "percentPktsNotReceiveInTime", ping.PercentPacketsNotReceivedInTime);
	}

	private static void PopulateWeb(WebCheckResource web, JObject device)
	{
		web.Domain = GetString(device, "domain", web.HostName);
		web.Scheme = SchemeFromWire(GetString(device, "schema"), web.Scheme);
		web.IgnoreSsl = GetBool(device, "ignoreSSL", false);
		web.PageLoadAlertTimeMs = GetInt(device, "pageLoadAlertTimeInMS", web.PageLoadAlertTimeMs);
		web.TriggerSslStatusAlerts = GetBool(device, "triggerSSLStatusAlert", false);
		web.TriggerSslExpirationAlerts = GetBool(device, "triggerSSLExpirationAlert", false);
		web.AlertExpression = GetString(device, "alertExpr");

		if (device["steps"] is JArray steps)
		{
			web.Steps = steps.ToObject<List<UptimeWebCheckStep>>() ?? [];
		}
	}

	private static Dictionary<string, string> ReadCustomProperties(JObject device)
	{
		var result = new Dictionary<string, string>(StringComparer.Ordinal);
		if (device["customProperties"] is not JArray array)
		{
			return result;
		}

		foreach (var property in array)
		{
			var name = property["name"]?.ToString();
			if (!string.IsNullOrEmpty(name))
			{
				result[name!] = property["value"]?.ToString() ?? string.Empty;
			}
		}

		return result;
	}
}
